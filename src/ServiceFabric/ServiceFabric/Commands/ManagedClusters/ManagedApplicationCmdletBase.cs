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
using System.Management.Automation;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.Core;
using Azure;
using System.Collections;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    public class ManagedApplicationCmdletBase : ServiceFabricManagedCmdletBase
    {
        #region TEST
        internal static bool RunningTest = false;
        #endregion

        /// <summary>
        /// Resource group name
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public virtual string ResourceGroupName { get; set; }

        /// <summary>
        /// Cluster name
        /// </summary>
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Specify the name of the cluster.")]
        [ValidateNotNullOrEmpty]
        public virtual string ClusterName { get; set; }

        protected ServiceFabricManagedApplicationTypeResource CreateManagedApplicationType(
            string applicationTypeName, 
            string location, 
            Hashtable tags, 
            bool errorIfPresent = true)
        {
            var sfManagedAppTypeCollection = this.GetSfManagedApplicationTypeCollection();
            var exists = sfManagedAppTypeCollection.ExistsAsync(applicationTypeName).GetAwaiter().GetResult().Value;
            if(exists)
            {
                if (errorIfPresent)
                {
                    WriteError(new ErrorRecord(new InvalidOperationException($"Managed app type '{applicationTypeName}' already exists."),
                        "ResourceAlreadyExists", ErrorCategory.InvalidOperation, null));
                }
                else
                {
                    WriteVerbose($"Managed app type '{applicationTypeName}' already exists.");
                }

                return sfManagedAppTypeCollection.GetAsync(applicationTypeName).GetAwaiter().GetResult().Value;
            }
            else
            {
                var data = new ServiceFabricManagedApplicationTypeData(new AzureLocation(location));
                if (tags != null)
                {
                    this.AddToList(data.Tags, tags);
                }

                var operation = sfManagedAppTypeCollection.CreateOrUpdateAsync(WaitUntil.Completed, applicationTypeName, data).GetAwaiter().GetResult();
                var sfManagedAppTypeResource =  operation.Value;
                return sfManagedAppTypeResource;
            }
        }

        protected ServiceFabricManagedApplicationTypeVersionResource CreateManagedApplicationTypeVersion(
            string applicationTypeName, 
            string typeVersion, 
            string location, 
            string packageUrl, 
            bool force, 
            Hashtable tags)
        {
            ServiceFabricManagedApplicationTypeVersionResource applicationTypeVersionResource = null;
            var sfManagedAppTypeVersionCollection = this.GetSfManagedApplicationTypeVersionCollection(applicationTypeName);
            var exists = sfManagedAppTypeVersionCollection.ExistsAsync(typeVersion).GetAwaiter().GetResult().Value;

            if (exists)
            {
                applicationTypeVersionResource = sfManagedAppTypeVersionCollection.GetAsync(typeVersion).GetAwaiter().GetResult().Value;
                if (applicationTypeVersionResource.Data?.ProvisioningState == "Failed")
                {
                    WriteVerbose($"Managed application type version '{applicationTypeName}':{typeVersion} already exists.");
                    string resourceMessage = $"Managed ApplicationTypeVersion {applicationTypeName}:{typeVersion}";
                    ConfirmAction(force,
                        $"{resourceMessage} already exits but provisioning is in Failed state. Do you want to recreate the resource?",
                        "Recreating managed application type version.",
                        resourceMessage,
                        () =>
                        {
                            applicationTypeVersionResource = CreateOrUpdateApplicationTypeVersion(
                                sfManagedAppTypeVersionCollection,
                                applicationTypeName: applicationTypeName,
                                typeVersion: typeVersion,
                                location: location,
                                packageUrl: packageUrl,
                                tags: tags);
                        });
                }
                else
                {
                    WriteError(new ErrorRecord(new InvalidOperationException($"Managed app type version '{typeVersion}' already exists."),
                        "ResourceAlreadyExists", ErrorCategory.InvalidOperation, null));
                }
            }
            else
            {
                applicationTypeVersionResource = CreateOrUpdateApplicationTypeVersion(
                    sfManagedAppTypeVersionCollection,
                    applicationTypeName: applicationTypeName,
                    typeVersion: typeVersion,
                    location: location,
                    packageUrl: packageUrl,
                    tags: tags);
            }

            if (applicationTypeVersionResource?.Data.ProvisioningState == "Failed")
            {
                throw new PSInvalidOperationException($"Managed ApplicationTypeVersion {applicationTypeName}:{typeVersion} is in provisioning state {applicationTypeVersionResource.Data.ProvisioningState}");
            }

            return applicationTypeVersionResource;
        }

        private ServiceFabricManagedApplicationTypeVersionResource CreateOrUpdateApplicationTypeVersion(
            ServiceFabricManagedApplicationTypeVersionCollection sfManagedAppTypeVersionCollection, 
            string applicationTypeName, 
            string typeVersion, string location, 
            string packageUrl, 
            Hashtable tags)
        {
            WriteVerbose($"Creating managed app type version '{applicationTypeName}':{typeVersion}.");

            var sfManagedAppTypeVersionData = new ServiceFabricManagedApplicationTypeVersionData(new AzureLocation(location))
            {
                AppPackageUri = new Uri(packageUrl),
            };

            if (tags != null)
            {
                this.AddToList(sfManagedAppTypeVersionData.Tags, tags);
            }


            var operation = sfManagedAppTypeVersionCollection.CreateOrUpdateAsync(WaitUntil.Completed, typeVersion, sfManagedAppTypeVersionData).GetAwaiter().GetResult();
            var result = operation.Value;

            return result;
        }

        protected ServiceFabricManagedApplicationTypeCollection GetSfManagedApplicationTypeCollection()
        {
            var serviceFabricManagedClusterResource = GetManagedClusterResource(this.ResourceGroupName, this.ClusterName);
            var sfManagedAppTypeCollection = serviceFabricManagedClusterResource.GetServiceFabricManagedApplicationTypes();

            return sfManagedAppTypeCollection;
        }

        protected ServiceFabricManagedApplicationTypeVersionCollection GetSfManagedApplicationTypeVersionCollection( string applicationName)
        {
            var serviceFabricManagedApplicationTypeResourceId = ServiceFabricManagedApplicationTypeResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id,
                this.ResourceGroupName,
                this.ClusterName,
                applicationName);

            var sfAppTypeResource = this.ArmClient.GetServiceFabricManagedApplicationTypeResource(serviceFabricManagedApplicationTypeResourceId);
            var sfManagedAppTypeVersionCollection = sfAppTypeResource.GetServiceFabricManagedApplicationTypeVersions();

            return sfManagedAppTypeVersionCollection;
        }

        protected ServiceFabricManagedApplicationCollection GetSfManagedApplicationCollection() 
        {
            var sfManagedClusterResource = GetManagedClusterResource(this.ResourceGroupName, this.ClusterName);
            var sfManagedApplicationCollection = sfManagedClusterResource.GetServiceFabricManagedApplications();
            return sfManagedApplicationCollection;
        }

        protected ServiceFabricManagedServiceCollection GetSfManagedServiceCollection(string applicationName)
        {
            var serviceFabricManagedApplicationResourceId = ServiceFabricManagedApplicationResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id, 
                this.ResourceGroupName, 
                this.ClusterName, 
                applicationName);

            var sfManagedAppResource = this.ArmClient.GetServiceFabricManagedApplicationResource(serviceFabricManagedApplicationResourceId);
            var sfManagedServiceCollection = sfManagedAppResource.GetServiceFabricManagedServices();

            return sfManagedServiceCollection;
        }
    }
}
