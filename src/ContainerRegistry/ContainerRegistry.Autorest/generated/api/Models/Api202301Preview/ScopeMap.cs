// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

namespace Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview
{
    using static Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Extensions;

    /// <summary>An object that represents a scope map for a container registry.</summary>
    public partial class ScopeMap :
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMap,
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal,
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IValidates,
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IHeaderSerializable
    {
        /// <summary>
        /// Backing field for Inherited model <see cref= "Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResource"
        /// />
        /// </summary>
        private Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResource __proxyResource = new Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.ProxyResource();

        /// <summary>
        /// The list of scoped permissions for registry artifacts.
        /// E.g. repositories/repository-name/content/read,
        /// repositories/repository-name/metadata/write
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.FormatTable(Index = 1)]
        public string[] Action { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Action; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Action = value ?? null /* arrayOf */; }

        /// <summary>Backing field for <see cref="AzureAsyncOperation" /> property.</summary>
        private string _azureAsyncOperation;

        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Owned)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string AzureAsyncOperation { get => this._azureAsyncOperation; set => this._azureAsyncOperation = value; }

        /// <summary>The creation date of scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public global::System.DateTime? CreationDate { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).CreationDate; }

        /// <summary>The user friendly description of the scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string Description { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Description; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Description = value ?? null; }

        /// <summary>The resource ID.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string Id { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Id; }

        /// <summary>Internal Acessors for Id</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal.Id { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Id; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Id = value; }

        /// <summary>Internal Acessors for Name</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal.Name { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Name; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Name = value; }

        /// <summary>Internal Acessors for SystemData</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.ISystemData Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal.SystemData { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemData; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemData = value; }

        /// <summary>Internal Acessors for Type</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal.Type { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Type; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Type = value; }

        /// <summary>Internal Acessors for CreationDate</summary>
        global::System.DateTime? Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal.CreationDate { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).CreationDate; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).CreationDate = value; }

        /// <summary>Internal Acessors for PropertiesType</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal.PropertiesType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Type; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Type = value; }

        /// <summary>Internal Acessors for Property</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapProperties Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal.Property { get => (this._property = this._property ?? new Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ScopeMapProperties()); set { {_property = value;} } }

        /// <summary>Internal Acessors for ProvisioningState</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.ProvisioningState? Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal.ProvisioningState { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).ProvisioningState; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).ProvisioningState = value; }

        /// <summary>The name of the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.FormatTable(Index = 0)]
        public string Name { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Name; }

        /// <summary>The type of the scope map. E.g. BuildIn scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string PropertiesType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).Type; }

        /// <summary>Backing field for <see cref="Property" /> property.</summary>
        private Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapProperties _property;

        /// <summary>The properties of the scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Owned)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        internal Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapProperties Property { get => (this._property = this._property ?? new Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ScopeMapProperties()); set => this._property = value; }

        /// <summary>Provisioning state of the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.ProvisioningState? ProvisioningState { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapPropertiesInternal)Property).ProvisioningState; }

        /// <summary>Gets the resource group name</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Owned)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string ResourceGroupName { get => (new global::System.Text.RegularExpressions.Regex("^/subscriptions/(?<subscriptionId>[^/]+)/resourceGroups/(?<resourceGroupName>[^/]+)/providers/", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase).Match(this.Id).Success ? new global::System.Text.RegularExpressions.Regex("^/subscriptions/(?<subscriptionId>[^/]+)/resourceGroups/(?<resourceGroupName>[^/]+)/providers/", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase).Match(this.Id).Groups["resourceGroupName"].Value : null); }

        /// <summary>Metadata pertaining to creation and last modification of the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.ISystemData SystemData { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemData; }

        /// <summary>The timestamp of resource creation (UTC).</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public global::System.DateTime? SystemDataCreatedAt { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedAt; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedAt = value ?? default(global::System.DateTime); }

        /// <summary>The identity that created the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string SystemDataCreatedBy { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedBy; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedBy = value ?? null; }

        /// <summary>The type of identity that created the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CreatedByType? SystemDataCreatedByType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedByType; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataCreatedByType = value ?? ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CreatedByType)""); }

        /// <summary>The timestamp of resource modification (UTC).</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public global::System.DateTime? SystemDataLastModifiedAt { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedAt; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedAt = value ?? default(global::System.DateTime); }

        /// <summary>The identity that last modified the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public string SystemDataLastModifiedBy { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedBy; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedBy = value ?? null; }

        /// <summary>The type of identity that last modified the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.DoNotFormat]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.LastModifiedByType? SystemDataLastModifiedByType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedByType; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).SystemDataLastModifiedByType = value ?? ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.LastModifiedByType)""); }

        /// <summary>The type of the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inherited)]
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.FormatTable(Index = 2)]
        public string Type { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal)__proxyResource).Type; }

        /// <param name="headers"></param>
        void Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IHeaderSerializable.ReadHeaders(global::System.Net.Http.Headers.HttpResponseHeaders headers)
        {
            if (headers.TryGetValues("Azure-AsyncOperation", out var __azureAsyncOperationHeader0))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapInternal)this).AzureAsyncOperation = System.Linq.Enumerable.FirstOrDefault(__azureAsyncOperationHeader0) is string __headerAzureAsyncOperationHeader0 ? __headerAzureAsyncOperationHeader0 : (string)null;
            }
        }

        /// <summary>Creates an new <see cref="ScopeMap" /> instance.</summary>
        public ScopeMap()
        {

        }

        /// <summary>Validates that this object meets the validation criteria.</summary>
        /// <param name="eventListener">an <see cref="Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IEventListener" /> instance that will receive validation
        /// events.</param>
        /// <returns>
        /// A <see cref = "global::System.Threading.Tasks.Task" /> that will be complete when validation is completed.
        /// </returns>
        public async global::System.Threading.Tasks.Task Validate(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IEventListener eventListener)
        {
            await eventListener.AssertNotNull(nameof(__proxyResource), __proxyResource);
            await eventListener.AssertObjectIsValid(nameof(__proxyResource), __proxyResource);
        }
    }
    /// An object that represents a scope map for a container registry.
    public partial interface IScopeMap :
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IJsonSerializable,
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResource
    {
        /// <summary>
        /// The list of scoped permissions for registry artifacts.
        /// E.g. repositories/repository-name/content/read,
        /// repositories/repository-name/metadata/write
        /// </summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = false,
        Description = @"The list of scoped permissions for registry artifacts.
        E.g. repositories/repository-name/content/read,
        repositories/repository-name/metadata/write",
        SerializedName = @"actions",
        PossibleTypes = new [] { typeof(string) })]
        string[] Action { get; set; }

        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = false,
        Description = @"",
        SerializedName = @"Azure-AsyncOperation",
        PossibleTypes = new [] { typeof(string) })]
        string AzureAsyncOperation { get; set; }
        /// <summary>The creation date of scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"The creation date of scope map.",
        SerializedName = @"creationDate",
        PossibleTypes = new [] { typeof(global::System.DateTime) })]
        global::System.DateTime? CreationDate { get;  }
        /// <summary>The user friendly description of the scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = false,
        Description = @"The user friendly description of the scope map.",
        SerializedName = @"description",
        PossibleTypes = new [] { typeof(string) })]
        string Description { get; set; }
        /// <summary>The type of the scope map. E.g. BuildIn scope map.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"The type of the scope map. E.g. BuildIn scope map.",
        SerializedName = @"type",
        PossibleTypes = new [] { typeof(string) })]
        string PropertiesType { get;  }
        /// <summary>Provisioning state of the resource.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"Provisioning state of the resource.",
        SerializedName = @"provisioningState",
        PossibleTypes = new [] { typeof(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.ProvisioningState) })]
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.ProvisioningState? ProvisioningState { get;  }

    }
    /// An object that represents a scope map for a container registry.
    internal partial interface IScopeMapInternal :
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api20190601Preview.IProxyResourceInternal
    {
        /// <summary>
        /// The list of scoped permissions for registry artifacts.
        /// E.g. repositories/repository-name/content/read,
        /// repositories/repository-name/metadata/write
        /// </summary>
        string[] Action { get; set; }

        string AzureAsyncOperation { get; set; }
        /// <summary>The creation date of scope map.</summary>
        global::System.DateTime? CreationDate { get; set; }
        /// <summary>The user friendly description of the scope map.</summary>
        string Description { get; set; }
        /// <summary>The type of the scope map. E.g. BuildIn scope map.</summary>
        string PropertiesType { get; set; }
        /// <summary>The properties of the scope map.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.IScopeMapProperties Property { get; set; }
        /// <summary>Provisioning state of the resource.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.ProvisioningState? ProvisioningState { get; set; }

    }
}