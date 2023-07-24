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
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.Common.Compute.Version_2018_04;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.ResourceManager.Version2018_05_01;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.ServiceFabricManagedClusters.Models;
using Microsoft.Azure.Commands.Common.Strategies;
using Azure.Core;
using Azure.ResourceManager;
using Azure;
using Microsoft.Azure.Management.ServiceFabric.Models;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;
using Azure.ResourceManager.Resources.Models;

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

        protected ServiceFabricManagedApplicationTypeResource CreateManagedApplicationType(string applicationTypeName, string location, KeyValuePair<string, string> tags, bool errorIfPresent = true)
        {
            ResourceIdentifier serviceFabricManagedApplicationTypeResourceId = ServiceFabricManagedApplicationTypeResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id, 
                this.ResourceGroupName, 
                this.ClusterName,
                applicationTypeName); 

            ServiceFabricManagedApplicationTypeResource applicationType = SafeGetResource(() => 
                this.ArmClient.GetServiceFabricManagedApplicationTypeResource(serviceFabricManagedApplicationTypeResourceId));

            if (applicationType != null)
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

                return applicationType;
            }
            else
            {
                ServiceFabricManagedClusterResource serviceFabricManagedCluster = GetManagedClusterResource(this.ResourceGroupName, this.ClusterName);
                ServiceFabricManagedApplicationTypeCollection collection = serviceFabricManagedCluster.GetServiceFabricManagedApplicationTypes();

                var data = new ServiceFabricManagedApplicationTypeData(new AzureLocation(location));
                data.Tags.Add(tags);

                ArmOperation<ServiceFabricManagedApplicationTypeResource> lro = collection.CreateOrUpdateAsync(WaitUntil.Completed, applicationTypeName, data).GetAwaiter().GetResult();
                var result =  lro.Value;
                return result;
            }
        }

        protected ServiceFabricManagedApplicationTypeVersionResource CreateManagedApplicationTypeVersion(string applicationTypeName, string typeVersion, string location, string packageUrl, bool force, KeyValuePair<string, string> tags)
        {
            ResourceIdentifier serviceFabricManagedApplicationTypeVersionResourceId = ServiceFabricManagedApplicationTypeVersionResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id, 
                this.ResourceGroupName, 
                this.ClusterName, 
                applicationTypeName,
                typeVersion);
             
            ServiceFabricManagedApplicationTypeVersionResource applicationTypeVersion = SafeGetResource(() => 
                this.ArmClient.GetServiceFabricManagedApplicationTypeVersionResource( serviceFabricManagedApplicationTypeVersionResourceId));


            if (applicationTypeVersion != null)
            {
                if (applicationTypeVersion.Data?.ProvisioningState == "Failed")
                {
                    WriteVerbose($"Managed application type version '{applicationTypeName}':{typeVersion} already exists.");
                    string resourceMessage = $"Managed ApplicationTypeVersion {applicationTypeName}:{typeVersion}";
                    ConfirmAction(force,
                        $"{resourceMessage} already exits but provisioning is in Failed state. Do you want to recreate the resource?",
                        "Recreating managed application type version.",
                        resourceMessage,
                        () =>
                        {
                            applicationTypeVersion = CreateOrUpdateApplicationTypeVersion(
                                applicationTypeName: applicationTypeName,
                                typeVersion: typeVersion,
                                location: location,
                                packageUrl: packageUrl,
                                tags: tags);
                        });
                }
                else
                {
                    WriteError(new ErrorRecord(new InvalidOperationException($"Managed app type version '{applicationTypeName}' already exists."),
                        "ResourceAlreadyExists", ErrorCategory.InvalidOperation, null));
                }
            }
            else
            {
                applicationTypeVersion = CreateOrUpdateApplicationTypeVersion(
                    applicationTypeName: applicationTypeName,
                    typeVersion: typeVersion,
                    location: location,
                    packageUrl: packageUrl,
                    tags: tags);
            }

            if (applicationTypeVersion?.Data.ProvisioningState == "Failed")
            {
                throw new PSInvalidOperationException($"Managed ApplicationTypeVersion {applicationTypeName}:{typeVersion} is in provisioning state {applicationTypeVersion.Data.ProvisioningState}");
            }

            return applicationTypeVersion;
        }

        private ServiceFabricManagedApplicationTypeVersionResource CreateOrUpdateApplicationTypeVersion(string applicationTypeName, string typeVersion, string location, string packageUrl, KeyValuePair<string, string> tags)
        {
            WriteVerbose($"Creating managed app type version '{applicationTypeName}':{typeVersion}.");

            var serviceFabricManagedApplicationTypeResourceId = ServiceFabricManagedApplicationTypeResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id, 
                this.ResourceGroupName, 
                this.ClusterName, 
                applicationTypeName);

            var serviceFabricManagedApplicationType = this.ArmClient.GetServiceFabricManagedApplicationTypeResource(serviceFabricManagedApplicationTypeResourceId);

            ServiceFabricManagedApplicationTypeVersionCollection collection = serviceFabricManagedApplicationType.GetServiceFabricManagedApplicationTypeVersions();

            ServiceFabricManagedApplicationTypeVersionData data = new ServiceFabricManagedApplicationTypeVersionData(new AzureLocation(location))
            {
                AppPackageUri = new Uri(packageUrl),
            };

            data.Tags.Add(tags);


            ArmOperation<ServiceFabricManagedApplicationTypeVersionResource> lro = collection.CreateOrUpdateAsync(WaitUntil.Completed, typeVersion, data).GetAwaiter().GetResult();
            ServiceFabricManagedApplicationTypeVersionResource result = lro.Value;

            return result;
        }

        /*protected ServiceFabricManagedApplicationTypeVersionData GetApplicationTypeVersionParameters(
            string appTypeName, 
            string resourceGroupName,
            string clusterName, string packageUrl,
            KeyValuePair<string, string> tags,
            string location)
        {
            var serviceFabricManagedApplicationTypeResourceId = ServiceFabricManagedApplicationTypeResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id,
                resourceGroupName,
                clusterName,
                appTypeName);

        
            ServiceFabricManagedApplicationTypeVersionData data = new ServiceFabricManagedApplicationTypeVersionData(new AzureLocation(location))
            {
                AppPackageUri = new Uri(packageUrl),
            };

            data.Tags.Add(tags);

            return data;
        }*/
    }
}
