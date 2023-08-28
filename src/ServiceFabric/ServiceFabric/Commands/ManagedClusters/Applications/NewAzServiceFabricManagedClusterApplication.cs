// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Management.Automation;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterApplication", SupportsShouldProcess = true, DefaultParameterSetName = SkipAppTypeVersion), OutputType(typeof(ServiceFabricManagedApplicationData))]
    public class NewAzServiceFabricManagedClusterApplication : ManagedApplicationCmdletBase
    {
        protected const string SkipAppTypeVersion = "SkipAppTypeVersion";
        protected const string CreateAppTypeVersion = "CreateAppTypeVersion";

        protected const string AppTypeArmResourceIdFormat = "/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceFabric/managedclusters/{2}/applicationTypes/{3}/versions/{4}";

        #region Parameters
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = SkipAppTypeVersion, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = CreateAppTypeVersion, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public override string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = SkipAppTypeVersion, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = CreateAppTypeVersion, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public override string ClusterName { get; set; }

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = SkipAppTypeVersion,
            HelpMessage = "Specify the name of the managed application type")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = CreateAppTypeVersion,
            HelpMessage = "Specify the name of the managed application type")]
        [ValidateNotNullOrEmpty]
        public string ApplicationTypeName { get; set; }

        [Parameter(Mandatory = true, Position = 3, ParameterSetName = SkipAppTypeVersion,
            HelpMessage = "Specify the managed application type version")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = CreateAppTypeVersion,
            HelpMessage = "Specify the managed application type version")]
        [ValidateNotNullOrEmpty]
        public string ApplicationTypeVersion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = SkipAppTypeVersion,
            HelpMessage = "Specify the name of the managed application")]
        [Parameter(Mandatory = true, ParameterSetName = CreateAppTypeVersion,
            HelpMessage = "Specify the name of the managed application")]
        [ValidateNotNullOrEmpty]
        [Alias("ApplicationName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = SkipAppTypeVersion,
            HelpMessage = "Specify the application parameters as key/value pairs. These parameters must exist in the application manifest.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = CreateAppTypeVersion,
            HelpMessage = "Specify the application parameters as key/value pairs. These parameters must exist in the application manifest.")]
        [ValidateNotNullOrEmpty]
        public Hashtable ApplicationParameter { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = CreateAppTypeVersion,
            HelpMessage = "Specify the url of the application package sfpkg file")]
        [ValidateNotNullOrEmpty]
        public string PackageUrl { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = SkipAppTypeVersion, HelpMessage = "Specify the tags as key/value pairs.")]
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = CreateAppTypeVersion, HelpMessage = "Specify the tags as key/value pairs.")]
        public Hashtable Tag { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Continue without prompts")]
        public SwitchParameter Force { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (ShouldProcess(target: this.Name, action:
                $"Create new application. name {this.Name}, typename: {this.ApplicationTypeName}, version {this.ApplicationTypeVersion}, in resource group {this.ResourceGroupName}"))
            {
                try
                {
                    var sfManagedClusterCollection = this.GetServiceFabricManagedClusterCollection(this.ResourceGroupName);
                    var exists = sfManagedClusterCollection.ExistsAsync(this.ClusterName).GetAwaiter().GetResult().Value;
                    ServiceFabricManagedApplicationTypeVersionResource appTypeVersion = null;
                    if (!exists)
                    {
                        WriteError(new ErrorRecord(new InvalidOperationException($"Parent cluster '{this.ClusterName}' does not exist."),
                            "ResourceDoesNotExist", ErrorCategory.InvalidOperation, null));
                    }
                    else
                    {
                        var cluster = sfManagedClusterCollection.GetAsync(this.ClusterName).GetAwaiter().GetResult().Value;
                        if (ParameterSetName == CreateAppTypeVersion)
                        {
                            CreateManagedApplicationType(this.ApplicationTypeName, cluster.Data.Location, this.Tag);
                            appTypeVersion = CreateManagedApplicationTypeVersion(
                                applicationTypeName: this.ApplicationTypeName,
                                typeVersion: this.ApplicationTypeVersion,
                                location: cluster.Data.Location,
                                tags: this.Tag,
                                packageUrl: this.PackageUrl,
                                force: this.Force.IsPresent);
                        }

                        var managedApp = this.CreateManagedApplication(appTypeVersion.Id, cluster.Data.Location);
                        WriteObject(managedApp.Data, false);
                    }
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ServiceFabricManagedApplicationResource CreateManagedApplication(ResourceIdentifier versionId, string location)
        {
            var sfManagedApplicationCollection = this.GetSfManagedApplicationCollection();
            var appData = new ServiceFabricManagedApplicationData(new AzureLocation(location))
            {
                Version = versionId.ToString()
            };

            if (this.ApplicationParameter != null)
            {
                this.AddToList(appData.Parameters, this.ApplicationParameter);
            }

            if (this.Tag != null)
            {
                this.AddToList(appData.Tags, this.Tag);
            }
            
            var operation = sfManagedApplicationCollection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, appData).GetAwaiter().GetResult();
            var result = operation.Value;
            return result;
        }
    }
}
