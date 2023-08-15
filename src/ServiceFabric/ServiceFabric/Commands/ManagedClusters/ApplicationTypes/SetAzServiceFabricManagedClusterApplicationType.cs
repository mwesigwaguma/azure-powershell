// ----------------------------------------------------------------------------------
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
using System.Collections.Generic;
using System.Management.Automation;
using Azure;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterApplicationType", DefaultParameterSetName = ByResourceGroup, SupportsShouldProcess = true), OutputType(new Type[] { typeof(bool), typeof(PSManagedApplicationType) })]
    public class SetAzServiceFabricManagedClusterApplicationType : ManagedApplicationCmdletBase
    {
        private const string ByResourceGroup = "ByResourceGroup";
        private const string ByInputObject = "ByInputObject";
        private const string ByResourceId = "ByResourceId";

        #region Parameters
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ByResourceGroup, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public override string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ByResourceGroup, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public override string ClusterName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ByResourceGroup,
            HelpMessage = "Specify the name of the managed application type.")]
        [ValidateNotNullOrEmpty]
        [Alias("ApplicationTypeName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ByResourceGroup, HelpMessage = "Specify the tags as key/value pairs.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ByInputObject, HelpMessage = "Specify the tags as key/value pairs.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ByResourceId, HelpMessage = "Specify the tags as key/value pairs.")]
        public KeyValuePair<string, string> Tag { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ByResourceId, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Arm ResourceId of the managed application type.")]
        [ResourceIdCompleter(Constants.ManagedClustersFullType)]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ByInputObject, ValueFromPipeline = true,
            HelpMessage = "The managed application type resource.")]
        public PSManagedApplicationType InputObject { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ByResourceGroup, HelpMessage = "Remove without prompt.")]
        [Parameter(Mandatory = false, ParameterSetName = ByInputObject, HelpMessage = "Remove without prompt.")]
        [Parameter(Mandatory = false, ParameterSetName = ByResourceId, HelpMessage = "Remove without prompt.")]
        public SwitchParameter Force { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            try
            {
                this.SetParams();
                ServiceFabricManagedApplicationTypeData updatedAppTypeParams = null;
                switch (ParameterSetName)
                {
                    case ByResourceGroup:
                    case ByResourceId:
                        updatedAppTypeParams = this.GetUpdatedAppTypeParams();
                        break;
                    case ByInputObject:
                        updatedAppTypeParams = this.GetUpdatedAppTypeParams(this.InputObject);
                        break;
                    default:
                        throw new ArgumentException("Invalid parameter set", ParameterSetName);
                }

                if (updatedAppTypeParams != null && ShouldProcess(target: this.Name, action: $"Update managed app type name {this.Name}, cluster: {this.ClusterName} in resource group {this.ResourceGroupName}"))
                {
                    var sfManagedApplicationTypecollection = GetApplicationTypeCollection();
                    var operation = sfManagedApplicationTypecollection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, updatedAppTypeParams).GetAwaiter().GetResult();
                    var managedAppType = operation.Value;

                    WriteObject(managedAppType.Data);
                }
            }
            catch (Exception ex)
            {
                PrintSdkExceptionDetail(ex);
                throw;
            }
        }

        private ServiceFabricManagedApplicationTypeData GetUpdatedAppTypeParams(ServiceFabricManagedApplicationTypeData inputObject = null)
        {
            ServiceFabricManagedApplicationTypeData updatedAppType = null;

            if (inputObject == null)
            {
                var sfManagedApplicationTypecollection = GetApplicationTypeCollection();
                var exists = sfManagedApplicationTypecollection.ExistsAsync(this.Name).GetAwaiter().GetResult().Value;

                if (!exists)
                {
                    WriteError(new ErrorRecord(new InvalidOperationException($"Managed application type version '{this.Name}' does not exist."),
                        "ResourceDoesNotExist", ErrorCategory.InvalidOperation, null));
                    return null;
                }

                var currentAppType = sfManagedApplicationTypecollection.GetAsync(this.Name).GetAwaiter().GetResult().Value;
                updatedAppType = currentAppType.Data;
            }
            else
            {
                updatedAppType = inputObject;
            }

            if (this.IsParameterBound(c => c.Tag))
            {
                updatedAppType.Tags.Add(this.Tag);
            }

            return updatedAppType;
        }
         
        private void SetParams()
        {
            switch (ParameterSetName)
            {
                case ByResourceGroup:
                    break;
                case ByInputObject:
                    if (string.IsNullOrEmpty(this.InputObject?.Id))
                    {
                        throw new ArgumentException("ResourceId is null.");
                    }
                    SetParametersByResourceId(this.InputObject.Id);
                    break;
                case ByResourceId:
                    SetParametersByResourceId(this.ResourceId);
                    break;
            }
        }

        private void SetParametersByResourceId(string resourceId)
        {
            this.GetParametersByResourceId(resourceId, Constants.applicationTypeProvider, out string resourceGroup, out string resourceName, out string parentResourceName);
            this.ResourceGroupName = resourceGroup;
            this.Name = resourceName;
            this.ClusterName = parentResourceName;
        }
    }
}
