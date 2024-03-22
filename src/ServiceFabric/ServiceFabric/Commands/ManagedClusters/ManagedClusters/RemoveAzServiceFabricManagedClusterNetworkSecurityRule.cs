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
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Microsoft.Azure.Management.Internal.Resources;
using Microsoft.Azure.Management.ServiceFabricManagedClusters;
using Microsoft.Azure.Management.ServiceFabricManagedClusters.Models;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Remove, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterNetworkSecurityRule", DefaultParameterSetName = ByObj, SupportsShouldProcess = true), OutputType(typeof(PSManagedCluster))]
    public class RemoveAzServiceFabricManagedClusterNetworkSecurityRule : ServiceFabricManagedCmdletBase
    {
        protected const string ByName = "ByName";
        protected const string ByObj = "ByObj";

        #region Params

        #region Common params

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = ByName,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty()]
        public string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = ByName,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty()]
        public string ClusterName { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ByObj,
            HelpMessage = "cluster resource")]
        [ValidateNotNull]
        public PSManagedCluster InputObject { get; set; }

        #endregion

        [Parameter(Mandatory = true, HelpMessage = "Network Security Rule name.")]
        [Alias("NSGName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            this.SetParams();
            if (ShouldProcess(target: this.Name, action: string.Format("Remove Network security rule {0} from cluster {1}", this.Name, this.ClusterName)))
            {
                try
                {
                    ManagedCluster updatedClusterParams = this.GetClusterWithRemovedNSG();
                    var beginRequestResponse = this.SfrpMcClient.ManagedClusters.BeginCreateOrUpdateWithHttpMessagesAsync(this.ResourceGroupName, this.ClusterName, updatedClusterParams)
                        .GetAwaiter().GetResult();

                    var cluster = this.PollLongRunningOperation(beginRequestResponse);

                    if (this.PassThru)
                    {
                        WriteObject(true);
                    }
                    else
                    {
                        WriteObject(new PSManagedCluster(cluster), false);
                    }
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ManagedCluster GetClusterWithRemovedNSG()
        {
            ManagedCluster currentCluster = this.SfrpMcClient.ManagedClusters.Get(this.ResourceGroupName, this.ClusterName);

            if (currentCluster.NetworkSecurityRules != null)
            {
                var originalLength = currentCluster.NetworkSecurityRules.Count();
                currentCluster.NetworkSecurityRules = currentCluster.NetworkSecurityRules.Where(nsg => !string.Equals(nsg.Name, this.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                if (originalLength == currentCluster.NetworkSecurityRules.Count())
                {
                    throw new ArgumentException(string.Format("Network Security Rule with name {0} not found", this.Name));
                }
            }

            return currentCluster;
        }

        private void SetParams()
        {
            switch (ParameterSetName)
            {
                case ByObj:
                    if (string.IsNullOrEmpty(this.InputObject?.Id))
                    {
                        throw new ArgumentException("ResourceId is null.");
                    }

                    SetParametersByResourceId(this.InputObject.Id);
                    break;
            }
        }

        private void SetParametersByResourceId(string resourceId)
        {
            this.GetParametersByResourceId(resourceId, Constants.ManagedClusterProvider, out string resourceGroup, out string resourceName);
            this.ResourceGroupName = resourceGroup;
            this.Name = resourceName;
        }
    }
}
