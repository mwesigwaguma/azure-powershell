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
using System.Management.Automation;
using Azure.ResourceManager;
using Azure;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System.Collections;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedCluster", DefaultParameterSetName = ByObj, SupportsShouldProcess = true), OutputType(typeof(ServiceFabricManagedClusterData))]
    public class SetAzServiceFabricManagedCluster : ServiceFabricManagedCmdletBase
    {
        protected const string WithParamsByName = "WithParamsByName";
        protected const string WithParamsById = "ByNameById";
        protected const string ByObj = "ByObj";

        #region Params

        #region Common params

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = WithParamsByName,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty()]
        public string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = WithParamsByName,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty()]
        [Alias("ClusterName")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = WithParamsById,
            HelpMessage = "Managed Cluster resource id")]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ByObj,
            HelpMessage = "Managed Cluster resource")]
        [ValidateNotNull]
        public ServiceFabricManagedClusterData InputObject { get; set; }

        #endregion

        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Cluster code version upgrade mode. Automatic or Manual.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Cluster code version upgrade mode. Automatic or Manual.")]
        public Models.ClusterUpgradeMode? UpgradeMode { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Cluster code version. Only use if upgrade mode is Manual.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Cluster code version. Only use if upgrade mode is Manual.")]
        public string CodeVersion { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Port used for http connections to the cluster. Default: 19080.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Port used for http connections to the cluster. Default: 19080.")]
        public int? HttpGatewayConnectionPort { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Port used for client connections to the cluster. Default: 19000.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Port used for client connections to the cluster. Default: 19000.")]
        public int? ClientConnectionPort { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Cluster's dns name.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Cluster's dns name.")]
        public string DnsName { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        
        [Parameter(Mandatory = false, ParameterSetName = WithParamsByName, HelpMessage = "Specify the tags as key/value pairs.")]
        [Parameter(Mandatory = false, ParameterSetName = WithParamsById, HelpMessage = "Specify the tags as key/value pairs.")]
        public Hashtable Tag { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            this.SetParams();
            if (ShouldProcess(target: this.Name, action: string.Format("Update cluster {0} on resource group: {1}", this.Name, this.ResourceGroupName)))
            {
                try
                {
                    var sfManagedClusterCollection = this.GetServiceFabricManagedClusterCollection(this.ResourceGroupName);
                    ServiceFabricManagedClusterData updatedClusterParams = null;
                    switch (ParameterSetName)
                    {
                        case WithParamsByName:
                        case WithParamsById:
                            updatedClusterParams = this.GetUpdatedClusterParams(sfManagedClusterCollection);
                            break;
                        case ByObj:
                            updatedClusterParams = this.InputObject;
                            break;
                        default:
                            throw new ArgumentException("Invalid parameter set", ParameterSetName);
                    }

                    var operation = sfManagedClusterCollection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, updatedClusterParams).GetAwaiter().GetResult();
                    var result = operation.Value;

                    WriteObject(result.Data, false);
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ServiceFabricManagedClusterData GetUpdatedClusterParams(ServiceFabricManagedClusterCollection sfManagedClusterCollection)
        {
            var currentClusterResource = sfManagedClusterCollection.GetAsync(this.Name).GetAwaiter().GetResult().Value;
            var currentClusterData = currentClusterResource.Data;

            this.ValidateParams(currentClusterData);

            if (!string.IsNullOrEmpty(this.CodeVersion))
            {
                currentClusterData.ClusterCodeVersion = this.CodeVersion;
            }

            if (this.ClientConnectionPort.HasValue)
            {
                currentClusterData.ClientConnectionPort = ClientConnectionPort;
            }

            if (!string.IsNullOrEmpty(this.DnsName))
            {
                currentClusterData.DnsName = DnsName;
            }

            if (this.IsParameterBound(c => c.Tag))
            {
                this.AddToList(currentClusterData.Tags, this.Tag);
            }

            return currentClusterData;
        }

        private void ValidateParams(ServiceFabricManagedClusterData currentCluster)
        {
            if (this.UpgradeMode.HasValue)
            {
                if (this.UpgradeMode == Models.ClusterUpgradeMode.Manual)
                {
                    throw new PSArgumentException("Currently only upgrade mode Automatic is supported. Support for Manual mode will be added latter on.", "UpgradeMode");
                }
            }
            
            if (!string.IsNullOrEmpty(this.CodeVersion))
            {
                throw new PSArgumentException("Currently the cluster upgrade mode is set to Automatic and CodeVersion should only be used when upgrade mode is set to Manual.", "CodeVersion");

                // TODO: when manual is available add this validation
                /*
                Enum.TryParse(currentCluster.ClusterUpgradeMode, out ClusterUpgradeMode upgradeMode);
                if (upgradeMode == ClusterUpgradeMode.Automatic)
                {
                    throw new PSArgumentException("Currently the cluster upgrade mode is set to Automatic and CodeVersion should only be used when upgrade mode is set to Manual.", "CodeVersion");
                }
                */
            }
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

                case WithParamsById:
                    SetParametersByResourceId(this.ResourceId);
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
