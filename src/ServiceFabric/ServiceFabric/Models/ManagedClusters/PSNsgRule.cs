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

using Microsoft.Azure.Management.ServiceFabricManagedClusters.Models;

namespace Microsoft.Azure.Commands.ServiceFabric.Models
{
    public class PSNsgRule : NetworkSecurityRule
    {
        public PSNsgRule(NetworkSecurityRule nsgRule)
            : base(name: nsgRule.Name,
                  description: nsgRule.Description,
                  protocol: nsgRule.Protocol,
                  sourceAddressPrefix: nsgRule.SourceAddressPrefix,
                  sourceAddressPrefixes: nsgRule.SourceAddressPrefixes,
                  destinationAddressPrefix: nsgRule.DestinationAddressPrefix,
                  destinationAddressPrefixes: nsgRule.DestinationAddressPrefixes,
                  sourcePortRange: nsgRule.SourcePortRange,
                  sourcePortRanges: nsgRule.SourcePortRanges,
                  destinationPortRange: nsgRule.DestinationPortRange,
                  destinationPortRanges: nsgRule.DestinationPortRanges,
                  access: nsgRule.Access,
                  priority: nsgRule.Priority,
                  direction: nsgRule.Direction)
                   
        {
        }
    }
}
