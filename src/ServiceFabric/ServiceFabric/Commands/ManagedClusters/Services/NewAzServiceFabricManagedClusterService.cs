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
using System.Collections;
using System.Management.Automation;
using Azure;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.ServiceFabricManagedClusters.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzurePrefix + Constants.ServiceFabricPrefix + "ManagedClusterService", SupportsShouldProcess = true, DefaultParameterSetName = StatelessSingleton), OutputType(typeof(ServiceFabricManagedServiceData))]
    public class NewAzServiceFabricManagedClusterService : ManagedApplicationCmdletBase
    {
        private const string StatelessSingleton = "Stateless-Singleton";
        private const string StatelessUniformInt64 = "Stateless-UniformInt64Range";
        private const string StatelessNamed = "Stateless-Named";
        private const string StatefulSingleton = "Stateful-Singleton";
        private const string StatefulUniformInt64 = "Stateful-UniformInt64Range";
        private const string StatefulNamed = "Stateful-Named";

        #region Parameters
        #region common required params

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the name of the resource group.")]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the name of the resource group.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public override string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the name of the cluster.")]
        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the name of the cluster.")]
        [ResourceNameCompleter(Constants.ManagedClustersFullType, nameof(ResourceGroupName))]
        [ValidateNotNullOrEmpty]
        public override string ClusterName { get; set; }

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the name of the managed application.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the name of the managed application.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the name of the managed application.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the name of the managed application.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the name of the managed application.")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the name of the managed application.")]
        [ValidateNotNullOrEmpty]
        public string ApplicationName { get; set; }

        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the name of the managed service.")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the name of the managed service.")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the name of the managed service.")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the name of the managed service.")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the name of the managed service.")]
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the name of the managed service.")]
        [ValidateNotNullOrEmpty]
        [Alias("ServiceName")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the service type name of the managed application, should exist in the application manifest.")]
        [ValidateNotNullOrEmpty]
        [Alias("ServiceType")]
        public string Type { get; set; }

        #endregion

        #region Stateless params

        [Parameter(Mandatory = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Use for stateless service")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Use for stateless service")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Use for stateless service")]
        public SwitchParameter Stateless { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the instance count for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the instance count for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the instance count for the managed service")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(-1, int.MaxValue)]
        public int InstanceCount { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the minimum instance count for the managed service")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the minimum instance count for the managed service")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the minimum instance count for the managed service")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, int.MaxValue)]
        public int MinInstanceCount { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the minimum instance percentage for the managed service")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the minimum instance percentage for the managed service")] 
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the minimum instance percentage for the managed service")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, 100)]
        public int MinInstancePercentage { get; set; }

        #endregion

        #region Stateful params

        [Parameter(Mandatory = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Use for stateful service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Use for stateful service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Use for stateful service")]
        public SwitchParameter Stateful { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the target replica set size for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the target replica set size for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the target replica set size for the managed service")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(1, int.MaxValue)]
        public int TargetReplicaSetSize { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the min replica set size for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the min replica set size for the managed service")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the min replica set size for the managed service")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(1, int.MaxValue)]
        public int MinReplicaSetSize { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the target replica set size for the managed service")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the target replica set size for the managed service")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the target replica set size for the managed service")]
        public SwitchParameter HasPersistedState { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the replica restart wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the replica restart wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the replica restart wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [ValidateNotNullOrEmpty]
        public TimeSpan ReplicaRestartWaitDuration { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the quorum loss wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the quorum loss wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the quorum loss wait duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [ValidateNotNullOrEmpty]
        public TimeSpan QuorumLossWaitDuration { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the stand by replica duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the stand by replica duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the stand by replica duration for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [ValidateNotNullOrEmpty]
        public TimeSpan StandByReplicaKeepDuration { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the service placement time limit for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the service placement time limit for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the service placement time limit for the managed service. Duration represented in ISO 8601 format 'hh:mm:ss'")]
        [ValidateNotNullOrEmpty]
        public TimeSpan ServicePlacementTimeLimit { get; set; }
        #endregion

        #region common optional params

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [ValidateNotNullOrEmpty]
        public MoveCostEnum DefaultMoveCost { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [ValidateNotNullOrEmpty]
        public string PlacementConstraint { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        public ManagedServiceLoadMetric[] Metric { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the placement constraints of the managed service, as a string.")]
        public ManagedServiceCorrelation[] Correlation { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = StatelessSingleton,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatelessNamed,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulSingleton,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [Parameter(Mandatory = false, ParameterSetName = StatefulNamed,
            HelpMessage = "Specify the default cost for a move. Higher costs make it less likely that the Cluster Resource Manager will move the replica when trying to balance the cluster")]
        [ValidateNotNullOrEmpty]
        public ServicePackageActivationModeEnum ServicePackageActivationMode { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the tags as key/value pairs.")]
        public Hashtable Tag { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Continue without prompts")]
        public SwitchParameter Force { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background and return a Job to track progress.")]
        public SwitchParameter AsJob { get; set; }

        #region partition params

        [Parameter(Mandatory = true, ParameterSetName = StatelessSingleton,
            HelpMessage = "Indicates that the service uses the singleton partition scheme. Singleton partitions are typically used when the service does not require any additional routing.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulSingleton,
            HelpMessage = "Indicates that the service uses the singleton partition scheme. Singleton partitions are typically used when the service does not require any additional routing.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter PartitionSchemeSingleton { get; set; }

        #region UniformInt64
        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Indicates that the service uses the UniformInt64 partition scheme. This means that each partition owns a range of int64 keys.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Indicates that the service uses the UniformInt64 partition scheme. This means that each partition owns a range of int64 keys.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter PartitionSchemeUniformInt64 { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the number of partitions.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the number of partitions.")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, int.MaxValue)]
        public int PartitionCount { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the lower bound of the partition key range.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the lower bound of the partition key range.")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, int.MaxValue)]
        public long LowKey { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessUniformInt64,
            HelpMessage = "Specify the upper bound of the partition key range.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulUniformInt64,
            HelpMessage = "Specify the upper bound of the partition key range.")]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, int.MaxValue)]
        public long HighKey { get; set; }
        #endregion

        #region Named Partition
        [Parameter(Mandatory = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Indicates that the service uses the named partition scheme. Services using this model usually have data that can be bucketed, within a bounded set. Some common examples of data fields used as named partition keys would be regions, postal codes, customer groups, or other business boundaries.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Indicates that the service uses the named partition scheme. Services using this model usually have data that can be bucketed, within a bounded set. Some common examples of data fields used as named partition keys would be regions, postal codes, customer groups, or other business boundaries.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter PartitionSchemeNamed { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = StatelessNamed,
            HelpMessage = "Indicates that the service uses the named partition scheme. Services using this model usually have data that can be bucketed, within a bounded set. Some common examples of data fields used as named partition keys would be regions, postal codes, customer groups, or other business boundaries.")]
        [Parameter(Mandatory = true, ParameterSetName = StatefulNamed,
            HelpMessage = "Indicates that the service uses the named partition scheme. Services using this model usually have data that can be bucketed, within a bounded set. Some common examples of data fields used as named partition keys would be regions, postal codes, customer groups, or other business boundaries.")]
        public string[] PartitionName { get; set; }
        #endregion

        #endregion

        #endregion
        #endregion


        public override void ExecuteCmdlet()
        {
            if (ShouldProcess(target: this.Name, action: $"Create new Service. name {this.Name} in application: {this.ApplicationName}, cluster {this.ClusterName} in resource group {this.ResourceGroupName}"))
            {
                try
                {
                    var sfManagedClusterCollection = this.GetServiceFabricManagedClusterCollection(this.ResourceGroupName);
                    var exists = sfManagedClusterCollection.ExistsAsync(this.ClusterName).GetAwaiter().GetResult().Value;
                    if (!exists)
                    {
                        WriteError(new ErrorRecord(new InvalidOperationException($"Parent cluster '{this.ClusterName}' does not exist."),
                            "ResourceDoesNotExist", ErrorCategory.InvalidOperation, null));
                    }
                    else
                    {
                        var clusterResource = sfManagedClusterCollection.GetAsync(this.ClusterName).GetAwaiter().GetResult().Value;
                        var serviceData = CreateService(clusterResource.Data.Location);
                        WriteObject(serviceData, false);
                    }
                }
                catch (Exception ex)
                {
                    PrintSdkExceptionDetail(ex);
                    throw;
                }
            }
        }

        private ServiceFabricManagedServiceData CreateService(string location)
        {
            var serviceCollection = GetSfManagedServiceCollection(this.ApplicationName);
            var exists = serviceCollection.ExistsAsync(this.Name).GetAwaiter().GetResult().Value;
            if (exists)
            {
                WriteError(new ErrorRecord(new InvalidOperationException($"Managed Service '{this.Name}' already exists."),
                    "ResourceAlreadyExists", ErrorCategory.InvalidOperation, null));

                var serviceResource = serviceCollection.GetAsync(this.Name).GetAwaiter().GetResult().Value;
                return serviceResource.Data;
            }

            WriteVerbose($"Creating service '{this.Name}.'");

            ServiceFabricManagedServiceData serviceParams = GetNewServiceParameters(location);
            var operation = serviceCollection.CreateOrUpdateAsync(WaitUntil.Completed, this.Name, serviceParams).GetAwaiter().GetResult();
            
            return operation.Value.Data;
        }

        #region Helper methods
        private ServiceFabricManagedServiceData GetNewServiceParameters( string location)
        {
            var serviceData = new ServiceFabricManagedServiceData(location);

            if (this.IsParameterBound(c => c.Tag))
            {
                this.AddToList(serviceData.Tags, this.Tag);
            }
            
            switch (ParameterSetName)
            {
                case StatelessSingleton:
                case StatelessUniformInt64:
                case StatelessNamed:
                    StatelessServiceProperties statelessProperties = new StatelessServiceProperties(
                        this.Type,
                        this.SetPartitionDescription(),
                        this.InstanceCount);

                    // Optional
                    if (this.IsParameterBound(c => c.MinInstancePercentage))
                    {
                        statelessProperties.MinInstancePercentage = this.MinInstancePercentage;
                    }
                    if (this.IsParameterBound(c => c.MinInstanceCount))
                    {
                        statelessProperties.MinInstanceCount = this.MinInstanceCount;
                    }

                    serviceData.Properties = statelessProperties;
                    break;
                case StatefulSingleton:
                case StatefulUniformInt64:
                case StatefulNamed:
                    StatefulServiceProperties statefulProperties = new StatefulServiceProperties(this.Type, this.SetPartitionDescription());
                    if (this.IsParameterBound(c => c.ServicePlacementTimeLimit))
                    {
                        statefulProperties.ServicePlacementTimeLimit = this.ServicePlacementTimeLimit;
                    }
                    if (this.IsParameterBound(c => c.StandByReplicaKeepDuration))
                    {
                        statefulProperties.StandByReplicaKeepDuration = this.StandByReplicaKeepDuration;
                    }
                    if (this.IsParameterBound(c => c.QuorumLossWaitDuration))
                    {
                        statefulProperties.QuorumLossWaitDuration = this.QuorumLossWaitDuration;
                    }
                    if (this.IsParameterBound(c => c.ReplicaRestartWaitDuration))
                    {
                        statefulProperties.ReplicaRestartWaitDuration = this.ReplicaRestartWaitDuration;
                    }
                    if (this.IsParameterBound(c => c.HasPersistedState))
                    {
                        statefulProperties.HasPersistedState = this.HasPersistedState.ToBool();
                    }
                    if (this.IsParameterBound(c => c.MinReplicaSetSize))
                    {
                        statefulProperties.MinReplicaSetSize = this.MinReplicaSetSize;
                    }
                    if (this.IsParameterBound(c => c.TargetReplicaSetSize))
                    {
                        statefulProperties.TargetReplicaSetSize = this.TargetReplicaSetSize;
                    }

                    serviceData.Properties = statefulProperties;
                    break;
                default:
                    throw new PSArgumentException("Invalid ParameterSetName");
            }
            SetCommonProperties(serviceData.Properties);

            return serviceData;
        }

        private void SetCommonProperties(ManagedServiceProperties properties)
        {
            // Optional
            if (this.IsParameterBound(c => c.ServicePackageActivationMode))
            {
                properties.ServicePackageActivationMode = this.ServicePackageActivationMode.ToString();
            }
            if (this.IsParameterBound(c => c.DefaultMoveCost))
            {
                properties.DefaultMoveCost = this.DefaultMoveCost.ToString();
            }
            if (this.IsParameterBound(c => c.PlacementConstraint))
            {
                properties.PlacementConstraints = this.PlacementConstraint;
            }
            if (this.IsParameterBound(c => c.Metric))
            {
                foreach (ManagedServiceLoadMetric metric in this.Metric)
                {
                    properties.ServiceLoadMetrics.Add(metric);
                }
            }
            if (this.IsParameterBound(c => c.Correlation))
            {
                foreach (ManagedServiceCorrelation scheme in this.Correlation)
                { 
                    properties.CorrelationScheme.Add(scheme);
                }
            }
        }

        private ManagedServicePartitionScheme SetPartitionDescription()
        {
            switch (ParameterSetName)
            {
                case StatelessSingleton:
                case StatefulSingleton:
                    return new SingletonPartitionScheme();
                case StatelessUniformInt64:
                case StatefulUniformInt64:
                    return new UniformInt64RangePartitionScheme(this.PartitionCount, this.LowKey, this.HighKey);
                case StatelessNamed:
                case StatefulNamed:
                    return new NamedPartitionScheme(this.PartitionName);
                default:
                    throw new PSArgumentException("Invalid ParameterSetName");
            }
        }
        #endregion
    }
}
