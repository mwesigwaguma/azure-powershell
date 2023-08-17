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
using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.ServiceFabricManagedClusters.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Add, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterClientCertificate", DefaultParameterSetName = ClientCertByTpByObj, SupportsShouldProcess = true), OutputType(typeof(ServiceFabricManagedClusterData))]
    public class AddAzServiceFabricManagedClusterClientCertificate : ServiceFabricManagedCmdletBase
    {
        protected const string ClientCertByTpByName = "ClientCertByTpByName";
        protected const string ClientCertByCnByName = "ClientCertByCnByName";
        protected const string ClientCertByTpByObj = "ClientCertByTpByObj";
        protected const string ClientCertByCnByObj = "ClientCertByCnByObj";

        #region Params

        #region Common params

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = ClientCertByTpByName,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = ClientCertByCnByName,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty()]
        public string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = ClientCertByTpByName,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = ClientCertByCnByName,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty()]
        [Alias("ClusterName")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ClientCertByTpByObj,
            HelpMessage = "Managed cluster resource")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ClientCertByCnByObj,
            HelpMessage = "Managed cluster resource")]
        [ValidateNotNull]
        public PSManagedCluster InputObject { get; set; }

        #endregion

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTpByObj,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCnByObj,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByTpByName,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCnByName,
                   HelpMessage = "Use to specify if the client certificate has administrator level.")]
        public SwitchParameter Admin { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ClientCertByTpByName,
                   HelpMessage = "Client certificate thumbprint.")]
        [Parameter(Mandatory = true, ParameterSetName = ClientCertByTpByObj,
                   HelpMessage = "Client certificate thumbprint.")]
        [ValidateNotNullOrEmpty()]
        public string Thumbprint { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ClientCertByCnByName,
                   HelpMessage = "Client certificate common name.")]
        [Parameter(Mandatory = true, ParameterSetName = ClientCertByCnByObj,
                   HelpMessage = "Client certificate common name.")]
        [ValidateNotNullOrEmpty()]
        public string CommonName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCnByName,
                   HelpMessage = "List of Issuer thumbprints for the client certificate use comma to separate the issuers. Only use in combination with CommonName.")]
        [Parameter(Mandatory = false, ParameterSetName = ClientCertByCnByObj,
                   HelpMessage = "List of Issuer thumbprints for the client certificate use comma to separate the issuers. Only use in combination with CommonName.")]
        public string[] IssuerThumbprint { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            this.SetParams();
            if (ShouldProcess(target: this.Name, action: string.Format("Add client cert '{0}' to cluster: {1}.",
                (ParameterSetName == ClientCertByTpByName || ParameterSetName == ClientCertByTpByObj) ? this.Thumbprint : this.CommonName, this.Name)))
            {
                try
                {
                    ServiceFabricManagedClusterCollection collection = GetServiceFabricManagedClusterCollection(this.ResourceGroupName);
                    ServiceFabricManagedClusterData updatedCluster = this.GetClusterWithAddedClientCert(collection);

                    ArmOperation<ServiceFabricManagedClusterResource> lro = collection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, updatedCluster).GetAwaiter().GetResult();
                    ServiceFabricManagedClusterResource result = lro.Value;

                    WriteObject(result.Data, false);
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ServiceFabricManagedClusterData GetClusterWithAddedClientCert(ServiceFabricManagedClusterCollection collection)
        {
            ServiceFabricManagedClusterResource result = collection.GetAsync(this.Name).GetAwaiter().GetResult();
            ServiceFabricManagedClusterData currentCluster = result.Data;
            
            var newCert = new ManagedClusterClientCertificate(isAdmin: this.Admin.IsPresent);

            switch(ParameterSetName)
            {
                case ClientCertByTpByName:
                case ClientCertByTpByObj:
                    newCert.Thumbprint = BinaryData.FromString(this.Thumbprint);
                    break;
                case ClientCertByCnByName:
                case ClientCertByCnByObj:
                    newCert.CommonName = this.CommonName;
                    newCert.IssuerThumbprint = this.IssuerThumbprint != null ? BinaryData.FromString(string.Join(",", this.IssuerThumbprint)) : null;
                    break;
                default:
                    throw new ArgumentException("Invalid parameter set", ParameterSetName);
            }

            currentCluster.Clients.Add(newCert);

            return currentCluster;
        }

        private void SetParams()
        {
            switch (ParameterSetName)
            {
                case ClientCertByCnByObj:
                case ClientCertByTpByObj:
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
