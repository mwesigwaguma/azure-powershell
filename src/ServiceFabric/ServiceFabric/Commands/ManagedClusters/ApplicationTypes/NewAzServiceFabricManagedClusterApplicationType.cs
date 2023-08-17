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

using Azure.ResourceManager.ServiceFabricManagedClusters;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterApplicationType", SupportsShouldProcess = true), OutputType(typeof(ServiceFabricManagedApplicationTypeData))]
    public class NewAzServiceFabricManagedClusterApplicationType : ManagedApplicationCmdletBase
    {
        #region Parameters
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public override string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public override string ClusterName { get; set; }

        [Parameter(Mandatory = true, Position = 2, ValueFromPipeline = true,
            HelpMessage = "Specify the name of the application type")]
        [ValidateNotNullOrEmpty]
        [Alias("ApplicationTypeName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the tags as key/value pairs.")]
        public Hashtable Tag { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (ShouldProcess(target: this.Name, action: $"Create new managed application type {this.Name} in resource group {this.ResourceGroupName}"))
            {
                try
                {
                    var sfManagedClustercollection = GetServiceFabricManagedClusterCollection(this.ResourceGroupName);
                    var clusterExists = sfManagedClustercollection.ExistsAsync(this.Name).GetAwaiter().GetResult().Value;

                    if (!clusterExists)
                    {
                        WriteError(new ErrorRecord(new InvalidOperationException($"Parent cluster '{this.ClusterName}' does not exist."),
                            "ResourceDoesNotExist", ErrorCategory.InvalidOperation, null));
                    }
                    else
                    {
                        var cluster = sfManagedClustercollection.GetAsync(this.ClusterName).GetAwaiter().GetResult().Value;
                        var appType = CreateManagedApplicationType(this.Name, cluster.Data.Location, new KeyValuePair<string, string>(this.Tag.Keys.ToString(), this.Tag.Values.ToString()));
                        WriteObject(appType.Data);
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
}
