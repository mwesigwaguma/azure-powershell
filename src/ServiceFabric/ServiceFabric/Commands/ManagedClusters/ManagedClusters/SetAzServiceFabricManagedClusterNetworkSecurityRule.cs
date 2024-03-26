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
using System.Collections.Generic;
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
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterNetworkSecurityRule", DefaultParameterSetName = ByName, SupportsShouldProcess = true), OutputType(typeof(PSNsgRule))]
    public class SetAzServiceFabricManagedClusterNetworkSecurityRule : ServiceFabricManagedCmdletBase
    {
        protected const string ByName = "ByName";
        protected const string AnyTrueValue = "*";

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

        #endregion

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the network traffic is allowed or denied. Possible values include: Allow, Deny ")]
        public NetworkSecurityAccess? Access { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets network security rule description.")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the destination address prefixes. CIDR or destination IP ranges.")]
        public string[] DestinationAddressPrefix { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the destination port ranges.")]
        public string[] DestinationPortRange { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets network security rule direction. Possible values include: Inbound, Outbound ")]
        public NetworkSecurityDirection? Direction { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "network security rule name.")]
        [Alias("NetworkSecurityRuleName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the priority of the rule. The value can be in the range 1000 to 3000. Values outside this range are reserved for Service Fabric ManagerCluster Resource Provider. The priority number must be unique for each rule in the collection. The lower the priority number, the higher the priority of the rule.")]
        public int Priority { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets network protocol this rule applies to. Possible values include: http, https, tcp, udp, icmp, ah, esp, any ")]
        public NetworkSecurityProtocol? Protocol { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the CIDR or source IP ranges.")]
        public string[] SourceAddressPrefix { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Gets or sets the source port ranges.")]
        public string[] SourcePortRange { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            if (ShouldProcess(target: this.Name, action: string.Format("Add NetworkSecurityRule {0} to {1} cluster", this.Name, this.ClusterName)))
            {
                try
                {
                    ManagedCluster updatedCluster = this.GetClusterWithNewNetworkSecurityRule();
                    var beginRequestResponse = this.SfrpMcClient.ManagedClusters.BeginCreateOrUpdateWithHttpMessagesAsync(this.ResourceGroupName, this.ClusterName, updatedCluster)
                        .GetAwaiter().GetResult();

                    var cluster = this.PollLongRunningOperation(beginRequestResponse);
                    var nsg = cluster.NetworkSecurityRules.FirstOrDefault(x => x.Name == this.Name);

                    WriteObject(new PSNsgRule(nsg), false);
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ManagedCluster GetClusterWithNewNetworkSecurityRule()
        {
            ManagedCluster currentCluster = this.SfrpMcClient.ManagedClusters.Get(this.ResourceGroupName, this.ClusterName);

            var nsgToUpdate = currentCluster.NetworkSecurityRules.FirstOrDefault(ext => string.Equals(ext.Name, this.Name, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException(string.Format("NSG with name {0} not found", this.Name));
            currentCluster.NetworkSecurityRules = currentCluster.NetworkSecurityRules.Where(nsg => !string.Equals(nsg.Name, this.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (this.Access.HasValue)
            {
                nsgToUpdate.Access = this.Access.ToString();
            }

            if (!String.IsNullOrEmpty(this.Description))
            { 
                nsgToUpdate.Description = this.Description;
            }

            if (this.DestinationAddressPrefix != null)
            { 
                nsgToUpdate.DestinationAddressPrefixes = this.DestinationAddressPrefix;
            }

            if (this.DestinationPortRange != null)
            { 
                nsgToUpdate.DestinationPortRanges = this.DestinationPortRange;
            }

            if (this.Direction.HasValue)
            { 
                nsgToUpdate.Direction = this.Direction.ToString();
            }

            if (this.Protocol.HasValue)
            { 
                nsgToUpdate.Protocol = this.Protocol.ToString();
            }

            if (this.SourceAddressPrefix != null)
            { 
                nsgToUpdate.SourceAddressPrefixes = this.SourceAddressPrefix;
            }


            if (this.SourcePortRange != null)
            { 
                nsgToUpdate.SourcePortRanges = this.SourcePortRange;
            }

            currentCluster.NetworkSecurityRules.Add(nsgToUpdate);
            return currentCluster;

        }
    }
}