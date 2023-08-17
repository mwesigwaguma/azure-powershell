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
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.ServiceFabricManagedClusters.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;


namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Add, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedNodeTypeVMExtension", DefaultParameterSetName = ByObj, SupportsShouldProcess = true), OutputType(typeof(ServiceFabricManagedNodeTypeData))]
    public class AddAzServiceFabricManagedNodeTypeVMExtension : ServiceFabricManagedCmdletBase
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

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = ByName,
            HelpMessage = "Specify the name of the node type.")]
        [ValidateNotNullOrEmpty()]
        public string NodeTypeName { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ByObj,
            HelpMessage = "Node Type resource")]
        [ValidateNotNull]
        public ServiceFabricManagedNodeTypeData InputObject { get; set; }

        #endregion

        [Parameter(Mandatory = true, HelpMessage = "extension name.")]
        [Alias("ExtensionName")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "If a value is provided and is different from the previous value, the extension handler will be forced to update even if the extension configuration has not changed.")]
        public string ForceUpdateTag { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The name of the extension handler publisher. This can use the Get-AzVMImagePublisher cmdlet to get the publisher.")]
        public string Publisher { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specifies the type of the extension; an example is \"CustomScriptExtension\". You can use the Get-AzVMExtensionImageType cmdlet to get the extension type.")]
        public string Type { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specifies the version of the script handler.")]
        public string TypeHandlerVersion { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Indicates whether the extension should use a newer minor version if one is available at deployment time. Once deployed, however, the extension will not upgrade minor versions unless redeployed, even with this property set to true.")]
        public SwitchParameter AutoUpgradeMinorVersion { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Json formatted public settings for the extension.")]
        public Object Setting { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "The extension can contain either protectedSettings or protectedSettingsFromKeyVault or no protected settings at all.")]
        public Object ProtectedSetting { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Collection of extension names after which this extension needs to be provisioned.")]
        public string[] ProvisionAfterExtension { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            this.SetParams();
            if (ShouldProcess(target: this.Name, action: string.Format("Add Extensions {0} with type {1} to node type {2}", this.Name, this.Type, this.NodeTypeName)))
            {
                try
                {
                    var nodeTypeCollection = GetNodeTypeCollection(this.ResourceGroupName, this.ClusterName);
                    var updatedNodeTypeParams = this.GetNodeTypeWithAddedExtension(nodeTypeCollection);
                    var operation = nodeTypeCollection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, updatedNodeTypeParams).GetAwaiter().GetResult();

                    WriteObject(operation.Value.Data, false);
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ServiceFabricManagedNodeTypeData GetNodeTypeWithAddedExtension(ServiceFabricManagedNodeTypeCollection nodeTypeCollection)
        {
            var currentNodeTypeResource = nodeTypeCollection.GetAsync(this.Name).GetAwaiter().GetResult();
            var currentNodeType = currentNodeTypeResource.Value.Data;

            var extensionToAdd = new NodeTypeVmssExtension(this.Name, this.Publisher, this.Type, this.TypeHandlerVersion)
            {
                ForceUpdateTag = this.ForceUpdateTag,
                AutoUpgradeMinorVersion = this.AutoUpgradeMinorVersion.IsPresent,
                Settings = (BinaryData)this.Setting,
                ProtectedSettings = (BinaryData)this.ProtectedSetting
            };

            foreach(string provAfterExt in this.ProvisionAfterExtension)
            {
                extensionToAdd.ProvisionAfterExtensions.Add(provAfterExt);
            }

            currentNodeType.VmExtensions.Add(extensionToAdd);
            return currentNodeType;
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
            this.GetParametersByResourceId(resourceId, Constants.ManagedNodeTypeProvider, out string resourceGroup, out string resourceName, out string parentResourceName);
            this.ResourceGroupName = resourceGroup;
            this.Name = resourceName;
            this.ClusterName = parentResourceName;
        }
    }
}
