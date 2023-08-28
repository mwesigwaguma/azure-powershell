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

using Azure.ResourceManager;
using System;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.Resources;
using Azure.Core;
using Azure.Identity;
using System.Collections.Generic;
using System.Collections;

//using Azure.ResourceManager.ManagedServiceIdentities;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    public class ServiceFabricManagedCmdletBase : ServiceFabricCommonCmdletBase
    {
        private Lazy<ArmClient> armClient ;
        internal ArmClient ArmClient
        {
            get { return armClient.Value; }
            set { armClient = new Lazy<ArmClient>(() => value); }
        }

        public ServiceFabricManagedCmdletBase()
        {
            InitializeArmClient();
        }

        private void InitializeArmClient()
        {
            this.armClient = new Lazy<ArmClient> (() => new ArmClient(new DefaultAzureCredential(), this.DefaultContext.Subscription.Id));
        }

        #region Helper
        protected ServiceFabricManagedClusterResource GetManagedClusterResource(string resourceGroup, string clusterName)
        {
            var serviceFabricManagedClusterResourceId = ServiceFabricManagedClusterResource.CreateResourceIdentifier(
                             this.DefaultContext.Subscription.Id,
                             resourceGroup,
                             clusterName);

            var sfManagedClusterResource = this.ArmClient.GetServiceFabricManagedClusterResource(serviceFabricManagedClusterResourceId);
            return sfManagedClusterResource;
        }

        protected ServiceFabricManagedClusterCollection GetServiceFabricManagedClusterCollection(string resourceGroupName)
        {

            var resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id,
                resourceGroupName);

            var resourceGroupResource = this.ArmClient.GetResourceGroupResource(resourceGroupResourceId);
            var sfManagedClusterCollection = resourceGroupResource.GetServiceFabricManagedClusters();
            return sfManagedClusterCollection;
        }

        protected ServiceFabricManagedNodeTypeCollection GetNodeTypeCollection(string resourceGroupName,string clusterName)
        {
            var serviceFabricManagedClusterResource = GetManagedClusterResource(resourceGroupName, clusterName);
            var sfManagedNodetypeCollection = serviceFabricManagedClusterResource.GetServiceFabricManagedNodeTypes();
            return sfManagedNodetypeCollection;
        }

        protected void AddToList(IDictionary<string, string> currentList, Hashtable listToAdd)
        {
            foreach (DictionaryEntry entry in listToAdd)
            {
                currentList.Add(new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString()));
            }
        }

        #endregion
    }
}
