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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager;
using Azure;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.ServiceFabricManagedClusters.Models;
using System.Threading.Tasks;


namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedCluster", DefaultParameterSetName = ClientCertByTp, SupportsShouldProcess = true), OutputType(typeof(PSManagedCluster))]
    public class NewAzServiceFabricManagedCluster : ServiceFabricManagedCmdletBase
    {
        protected const string ClientCertByTp = "ClientCertByTp";
        protected const string ClientCertByCn = "ClientCertByCn";

        #region Params

        #region Common params

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ClientCertByTp, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ClientCertByCn, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty()]
        public string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ClientCertByTp, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ClientCertByCn, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty()]
        [Alias("ClusterName")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ClientCertByTp, HelpMessage = "The resource location")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ClientCertByCn, HelpMessage = "The resource location")]
        [LocationCompleter(Constants.ManagedClustersFullType)]
        public string Location { get; set; }

        #endregion

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Cluster service fabric code version upgrade mode. Automatic or Manual.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Cluster service fabric code version upgrade mode. Automatic or Manual.")]
        [Alias("ClusterUpgradeMode")]
        public ManagedClusterUpgradeMode UpgradeMode { get; set; } = ManagedClusterUpgradeMode.Automatic;

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Cluster service fabric code version. Only use if upgrade mode is Manual.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Cluster service fabric code version. Only use if upgrade mode is Manual.")]
        [ValidateNotNullOrEmpty()]
        [Alias("ClusterCodeVersion")]
        public string CodeVersion { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Indicates when new cluster runtime version upgrades will be applied after they are released. By default is Wave0.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Indicates when new cluster runtime version upgrades will be applied after they are released. By default is Wave0.")]
        [ValidateNotNullOrEmpty()]
        [Alias("ClusterUpgradeCadence")]
        public ManagedClusterUpgradeCadence UpgradeCadence { get; set; } = ManagedClusterUpgradeCadence.Wave0;

        #region Client cert params

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        public SwitchParameter ClientCertIsAdmin { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ClientCertByTp,
                   HelpMessage = "Client certificate thumbprint.")]
        [ValidateNotNullOrEmpty()]
        public string ClientCertThumbprint { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ClientCertByCn,
                   HelpMessage = "Client certificate common name.")]
        [ValidateNotNullOrEmpty()]
        public string ClientCertCommonName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn,
                   HelpMessage = "List of Issuer thumbprints for the client certificate. Only use in combination with ClientCertCommonName.")]
        public string[] ClientCertIssuerThumbprint { get; set; }

        #endregion

        [Parameter(Mandatory = true, ParameterSetName = ClientCertByTp, HelpMessage = "Admin password used for the virtual machines.")]
        [Parameter(Mandatory = true, ParameterSetName = ClientCertByCn, HelpMessage = "Admin password used for the virtual machines.")]
        [ValidateNotNullOrEmpty()]
        public SecureString AdminPassword { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Admin user used for the virtual machines. Default: vmadmin.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Admin user used for the virtual machines. Default: vmadmin.")]
        public string AdminUserName { get; set; } = "vmadmin";

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Port used for http connections to the cluster. Default: 19080.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Port used for http connections to the cluster. Default: 19080.")]
        public int HttpGatewayConnectionPort { get; set; } = 19080;

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Port used for client connections to the cluster. Default: 19000.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Port used for client connections to the cluster. Default: 19000.")]
        public int ClientConnectionPort { get; set; } = 19000;

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Cluster's dns name.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Cluster's dns name.")]
        public string DnsName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp,
            HelpMessage = "Cluster's Sku, the options are Basic: it will have a minimum of 3 seed nodes and only allows 1 node type and Standard: it will have a minimum of 5 seed nodes and allows multiple node types.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn,
            HelpMessage = "Cluster's Sku, the options are Basic: it will have a minimum of 3 seed nodes and only allows 1 node type and Standard: it will have a minimum of 5 seed nodes and allows multiple node types.")]
        public ManagedClusterSku Sku { get; set; } = ManagedClusterSku.Basic;

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "If Specify The cluster will be crated with service test vmss extension.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "If Specify The cluster will be crated with service test vmss extension.")]
        public SwitchParameter UseTestExtension { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Indicates if the cluster has zone resiliency.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Indicates if the cluster has zone resiliency.")]
        public SwitchParameter ZonalResiliency { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTp, HelpMessage = "Specify the tags as key/value pairs.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCn, HelpMessage = "Specify the tags as key/value pairs.")]
        public Hashtable Tag { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            if (ShouldProcess(target: this.Name, action: string.Format("Create new managed cluster {0} in resource group {1}", this.Name, this.ResourceGroupName)))
            {
                try
                {
                    SubscriptionResource subResource = this.ArmClient.GetDefaultSubscriptionAsync().GetAwaiter().GetResult();
                    ResourceGroupCollection resGroupCollection = subResource.GetResourceGroups();

                    var resGroupExists = resGroupCollection.ExistsAsync(this.ResourceGroupName).GetAwaiter().GetResult().Value;
                    ResourceGroupResource resourceGroupResource = null;

                    if (!resGroupExists)
                    {
                        resourceGroupResource = createResourceGroup(resGroupCollection).Result;
                    }
                    else
                    {
                        resourceGroupResource = resGroupCollection.GetAsync(this.ResourceGroupName).GetAwaiter().GetResult();
                    }

                    var result = this.createCluster(resourceGroupResource).Result;

                    WriteObject(result.Data, false);
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private async Task<ResourceGroupResource> createResourceGroup(ResourceGroupCollection resGroupCollection)
        {

            ArmOperation<ResourceGroupResource> operation = await resGroupCollection.CreateOrUpdateAsync(
                WaitUntil.Completed,
                this.ResourceGroupName,
                new ResourceGroupData(this.Location));

            return operation.Value;
        }

        private async Task<ServiceFabricManagedClusterResource> createCluster(ResourceGroupResource resourceGroupResource)
        {
            ServiceFabricManagedClusterCollection collection = resourceGroupResource.GetServiceFabricManagedClusters();
            var clsuterExists = collection.ExistsAsync(this.Name).GetAwaiter().GetResult().Value;
            
            if (clsuterExists)
            {
                WriteError(new ErrorRecord(new InvalidOperationException(string.Format("Cluster '{0}' already exists.", this.Name)),
                    "ResourceAlreadyExists", ErrorCategory.InvalidOperation, null));
            }

            ServiceFabricManagedClusterData newClusterParams = this.GetNewManagedClusterParameters();
            var operation = await collection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, newClusterParams);

            return operation.Value;
        }

        private ServiceFabricManagedClusterData GetNewManagedClusterParameters()
        {
            if (this.UpgradeMode == ManagedClusterUpgradeMode.Manual && string.IsNullOrEmpty(this.CodeVersion))
            {
                throw new PSArgumentException("UpgradeMode is set to manual but CodeVersion is not set. Please specify CodeVersion.", "CodeVersion");
            }

            if (this.UpgradeMode == ManagedClusterUpgradeMode.Automatic && !string.IsNullOrEmpty(this.CodeVersion))
            {
                throw new PSArgumentException("CodeVersion should only be used when upgrade mode is set to Manual.", "CodeVersion");
            }

            List<ManagedClusterClientCertificate> clientCerts = new List<ManagedClusterClientCertificate>();
            if (this.ParameterSetName == ClientCertByTp)
            {
                clientCerts.Add(new ManagedClusterClientCertificate(this.ClientCertIsAdmin.IsPresent)
                {
                    Thumbprint = BinaryData.FromString(this.ClientCertThumbprint),
                });
            }
            else if (this.ParameterSetName == ClientCertByCn)
            {
                clientCerts.Add(new ManagedClusterClientCertificate(this.ClientCertIsAdmin.IsPresent)
                {
                    CommonName = this.ClientCertCommonName,
                    IssuerThumbprint = this.ClientCertIssuerThumbprint != null ? BinaryData.FromString(string.Join(",", this.ClientCertIssuerThumbprint)) : null,
                });
            }

            if (string.IsNullOrEmpty(this.DnsName))
            {
                this.DnsName = this.Name;
            }

            var newCluster = new ServiceFabricManagedClusterData(location: this.Location)
            { 
                DnsName = this.DnsName,
                AdminUserName = this.AdminUserName,
                AdminPassword = this.AdminPassword.ToString(),
                HttpGatewayConnectionPort = this.HttpGatewayConnectionPort,
                ClientConnectionPort = this.ClientConnectionPort,
                SkuName = new ServiceFabricManagedClustersSkuName(value: this.Sku.ToString()),
                ClusterUpgradeMode = this.UpgradeMode,
                ClusterUpgradeCadence = this.UpgradeCadence,
                HasZoneResiliency = this.ZonalResiliency.IsPresent,
            };

            newCluster.Clients.Concat(clientCerts);
            newCluster.Tags.Add(new KeyValuePair<string, string>(this.Tag.Keys.ToString(), this.Tag.Values.ToString()));
           
            if (this.UpgradeMode == ManagedClusterUpgradeMode.Manual)
            {
                newCluster.ClusterCodeVersion = this.CodeVersion;
            }

            return newCluster;
        }
    }
}
