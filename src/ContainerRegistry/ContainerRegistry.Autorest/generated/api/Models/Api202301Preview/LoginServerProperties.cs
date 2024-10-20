// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

namespace Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview
{
    using static Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Extensions;

    /// <summary>The login server properties of the connected registry.</summary>
    public partial class LoginServerProperties :
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerProperties,
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal
    {

        /// <summary>Indicates the location of the certificates.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        public string CertificateLocation { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateLocation; }

        /// <summary>The type of certificate location.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CertificateType? CertificateType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateType; }

        /// <summary>Backing field for <see cref="Host" /> property.</summary>
        private string _host;

        /// <summary>The host of the connected registry. Can be FQDN or IP.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Owned)]
        public string Host { get => this._host; }

        /// <summary>Internal Acessors for CertificateLocation</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.CertificateLocation { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateLocation; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateLocation = value; }

        /// <summary>Internal Acessors for CertificateType</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CertificateType? Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.CertificateType { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateType; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).CertificateType = value; }

        /// <summary>Internal Acessors for Host</summary>
        string Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.Host { get => this._host; set { {_host = value;} } }

        /// <summary>Internal Acessors for Tl</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsProperties Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.Tl { get => (this._tl = this._tl ?? new Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.TlsProperties()); set { {_tl = value;} } }

        /// <summary>Internal Acessors for TlCertificate</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsCertificateProperties Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.TlCertificate { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).Certificate; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).Certificate = value; }

        /// <summary>Internal Acessors for TlStatus</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.TlsStatus? Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ILoginServerPropertiesInternal.TlStatus { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).Status; set => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).Status = value; }

        /// <summary>Backing field for <see cref="Tl" /> property.</summary>
        private Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsProperties _tl;

        /// <summary>The TLS properties of the connected registry login server.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Owned)]
        internal Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsProperties Tl { get => (this._tl = this._tl ?? new Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.TlsProperties()); }

        /// <summary>Indicates whether HTTPS is enabled for the login server.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Origin(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.PropertyOrigin.Inlined)]
        public Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.TlsStatus? TlStatus { get => ((Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsPropertiesInternal)Tl).Status; }

        /// <summary>Creates an new <see cref="LoginServerProperties" /> instance.</summary>
        public LoginServerProperties()
        {

        }
    }
    /// The login server properties of the connected registry.
    public partial interface ILoginServerProperties :
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.IJsonSerializable
    {
        /// <summary>Indicates the location of the certificates.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"Indicates the location of the certificates.",
        SerializedName = @"location",
        PossibleTypes = new [] { typeof(string) })]
        string CertificateLocation { get;  }
        /// <summary>The type of certificate location.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"The type of certificate location.",
        SerializedName = @"type",
        PossibleTypes = new [] { typeof(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CertificateType) })]
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CertificateType? CertificateType { get;  }
        /// <summary>The host of the connected registry. Can be FQDN or IP.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"The host of the connected registry. Can be FQDN or IP.",
        SerializedName = @"host",
        PossibleTypes = new [] { typeof(string) })]
        string Host { get;  }
        /// <summary>Indicates whether HTTPS is enabled for the login server.</summary>
        [Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Runtime.Info(
        Required = false,
        ReadOnly = true,
        Description = @"Indicates whether HTTPS is enabled for the login server.",
        SerializedName = @"status",
        PossibleTypes = new [] { typeof(Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.TlsStatus) })]
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.TlsStatus? TlStatus { get;  }

    }
    /// The login server properties of the connected registry.
    internal partial interface ILoginServerPropertiesInternal

    {
        /// <summary>Indicates the location of the certificates.</summary>
        string CertificateLocation { get; set; }
        /// <summary>The type of certificate location.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.CertificateType? CertificateType { get; set; }
        /// <summary>The host of the connected registry. Can be FQDN or IP.</summary>
        string Host { get; set; }
        /// <summary>The TLS properties of the connected registry login server.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsProperties Tl { get; set; }
        /// <summary>The certificate used to configure HTTPS for the login server.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Models.Api202301Preview.ITlsCertificateProperties TlCertificate { get; set; }
        /// <summary>Indicates whether HTTPS is enabled for the login server.</summary>
        Microsoft.Azure.PowerShell.Cmdlets.ContainerRegistry.Support.TlsStatus? TlStatus { get; set; }

    }
}