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

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Get, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterNetworkSecurityRule", DefaultParameterSetName = ByName), OutputType(typeof(PSNsgRule))]
    public class GetAzServiceFabricManagedClusterNetworkSecurityRule : ServiceFabricManagedCmdletBase
    {
        protected const string ByName = "ByName";

        #region Params

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


        [Parameter(Mandatory = false, HelpMessage = "network security rule name.")]
        [Alias("NetworkSecurityRuleName")]
        public string Name { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            try
            {
                var cluster = this.SfrpMcClient.ManagedClusters.Get(this.ResourceGroupName, this.ClusterName);

                if (!String.IsNullOrEmpty(this.Name))
                {
                    var nsgToReturn = cluster.NetworkSecurityRules.FirstOrDefault(nsg => string.Equals(nsg.Name, this.Name, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException(string.Format("NSG with name {0} not found", this.Name));
                    WriteObject(new PSNsgRule(nsgToReturn), false);
                }
                else 
                {
                    WriteObject(cluster.NetworkSecurityRules.Select(nsg => new PSNsgRule(nsg)), true);
                }
            }
            catch (Exception ex)
            {
                PrintSdkExceptionDetail(ex);
                throw;
            }
        }
    }
}
