// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Dns.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a single Azure resource and its referencing DNS records.
    /// </summary>
    public partial class DnsResourceReference
    {
        /// <summary>
        /// Initializes a new instance of the DnsResourceReference class.
        /// </summary>
        public DnsResourceReference()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DnsResourceReference class.
        /// </summary>
        /// <param name="dnsResources">A list of dns Records </param>
        /// <param name="targetResource">A reference to an azure resource from
        /// where the dns resource value is taken.</param>
        public DnsResourceReference(IList<SubResource> dnsResources = default(IList<SubResource>), SubResource targetResource = default(SubResource))
        {
            DnsResources = dnsResources;
            TargetResource = targetResource;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets a list of dns Records
        /// </summary>
        [JsonProperty(PropertyName = "dnsResources")]
        public IList<SubResource> DnsResources { get; set; }

        /// <summary>
        /// Gets or sets a reference to an azure resource from where the dns
        /// resource value is taken.
        /// </summary>
        [JsonProperty(PropertyName = "targetResource")]
        public SubResource TargetResource { get; set; }

    }
}
