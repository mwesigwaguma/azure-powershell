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

using Azure.ResourceManager;
using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Microsoft.Rest.Azure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Azure.ResourceManager.ServiceFabricManagedClusters;
using Azure.ResourceManager.Resources;
using Azure.Core;
using Azure.Identity;
//using Azure.ResourceManager.ManagedServiceIdentities;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    public class ServiceFabricManagedCmdletBase : ServiceFabricCommonCmdletBase
    {
        private const string ClusterResource = "Microsoft.ServiceFabric/managedClusters";
        private const string NodeTypeResource = "Microsoft.ServiceFabric/managedClusters/nodeTypes";
        private const string ApplicationResource = "Microsoft.ServiceFabric/managedclusters/applications";
        private const string ApplicationTypeResource = "Microsoft.ServiceFabric/managedclusters/applicationTypes";
        private const string ApplicationTypeVersionResource = "Microsoft.ServiceFabric/managedclusters/applicationTypes/versions";
        private const string ServiceResource = "Microsoft.ServiceFabric/managedclusters/applications/services";

        //private Lazy<ArmClient> armClient ;
        //protected SubscriptionResource DefaultSubscription;

       //protected TClient InstrumentClient<TClient>(TClient client, IEnumerable<IInterceptor> preInterceptors) where TClient : class;

        protected ResourceGroupCleanupPolicy ResourceGroupCleanupPolicy = new ResourceGroupCleanupPolicy();
        protected ResourceGroupCleanupPolicy OneTimeResourceGroupCleanupPolicy = new ResourceGroupCleanupPolicy();
        protected ManagementGroupCleanupPolicy ManagementGroupCleanupPolicy = new ManagementGroupCleanupPolicy();
        protected ManagementGroupCleanupPolicy OneTimeManagementGroupCleanupPolicy = new ManagementGroupCleanupPolicy();
        protected ResponseNullFilterPolicy NullFilterPolicy = new ResponseNullFilterPolicy();

        internal ArmClient ArmClient { get; set; }
        /*{
            get { return armClient.Value; }
            set { armClient = new Lazy<ArmClient>(() => value);}
        }*/

        public ServiceFabricManagedCmdletBase()
        {
            InitializeManagementClients();
        }

        private void InitializeManagementClients()
        {
            ArmClientOptions options = new ArmClientOptions();
            //options.SetApiVersion(UserAssignedIdentityResource.ResourceType, "2018-11-30");

            //options.AddPolicy(ResourceGroupCleanupPolicy, HttpPipelinePosition.PerCall);
            //options.AddPolicy(ManagementGroupCleanupPolicy, HttpPipelinePosition.PerCall);
            options.AddPolicy(NullFilterPolicy, HttpPipelinePosition.PerRetry);

            ArmClient = new ArmClient(new DefaultAzureCredential(), this.DefaultContext.Subscription.Id, options);
            //this.armClient = new Lazy<ArmClient> (() => new ArmClient(new DefaultAzureCredential(), this.DefaultContext.Subscription.Id, options));
        }

        #region Helper

       /* protected void PollLongRunningOperation(Rest.Azure.AzureOperationResponse beginRequestResponse)
        {
            AzureOperationResponse<object> response2 = new Rest.Azure.AzureOperationResponse<object>
            {
                Request = beginRequestResponse.Request,
                Response = beginRequestResponse.Response,
                RequestId = beginRequestResponse.RequestId
            };

            this.PollLongRunningOperation(response2);
        }

        protected T PollLongRunningOperation<T>(AzureOperationResponse<T> beginRequestResponse) where T : class
        {
            var progress = new ProgressRecord(0, "Request in progress", "Getting Status...");
            WriteProgress(progress);
            WriteVerboseWithTimestamp(string.Format("Begin request ARM correlationId: '{0}' response: '{1}'",
                                        beginRequestResponse.RequestId,
                                        beginRequestResponse.Response.StatusCode));

            AzureOperationResponse<T> result = null;
            var tokenSource = new CancellationTokenSource();
            Uri asyncOperationStatusEndpoint = null;
            HttpRequestMessage asyncOpStatusRequest = null;
            if (beginRequestResponse.Response.Headers.TryGetValues(Constants.AzureAsyncOperationHeader, out IEnumerable<string> headerValues))
            {
                asyncOperationStatusEndpoint = new Uri(headerValues.First());
                asyncOpStatusRequest = beginRequestResponse.Request;
            }

            var requestTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    result = this.SfrpMcClient.GetLongRunningOperationResultAsync(beginRequestResponse, null, CancellationToken.None).GetAwaiter().GetResult();
                }
                finally
                {
                    tokenSource.Cancel();
                }
            });

            
            while (!tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(WriteVerboseIntervalInSec));
                if (asyncOpStatusRequest != null && asyncOperationStatusEndpoint != null)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            asyncOpStatusRequest = this.CloneAndDisposeRequest(asyncOpStatusRequest, asyncOperationStatusEndpoint, HttpMethod.Get);
                            HttpResponseMessage responseJson = client.SendAsync(asyncOpStatusRequest).GetAwaiter().GetResult();
                            string content = responseJson.Content.ReadAsStringAsync().Result;
                            Operation op = this.ConvertToOperation(content);

                            if (op != null)
                            {
                                string progressMessage = $"Operation Status: {op.Status}. Progress: {op.PercentComplete} %";
                                WriteDebugWithTimestamp(progressMessage);
                                progress.StatusDescription = progressMessage;
                                progress.PercentComplete = Convert.ToInt32(op.PercentComplete);
                                WriteProgress(progress);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // don't throw if poll operation state fails
                        WriteDebugWithTimestamp("Error polling operation status {0}", ex);
                    }
                }
                else
                {
                    if (progress.StatusDescription != "In progress")
                    {
                        progress.StatusDescription = "In progress";
                        WriteProgress(progress);
                    }
                }
            }

            if (requestTask.IsFaulted)
            {
                var errorMessage = string.Format(
                    "Long Running Operation Failed. Begin request with ARM correlationId: '{0}' response: '{1}' operationId '{0}'",
                    beginRequestResponse.RequestId,
                    beginRequestResponse.Response.StatusCode,
                    this.GetOperationIdFromAsyncHeader(beginRequestResponse.Response.Headers));

                WriteErrorWithTimestamp(errorMessage);
                throw requestTask.Exception;
            }

            return result?.Body;
        }
*/
        private string GetOperationIdFromAsyncHeader(HttpResponseHeaders headers)
        {
            if (headers.Location != null)
            {
                return headers.Location.Segments.LastOrDefault();
            }

            if (headers.TryGetValues(Constants.AzureAsyncOperationHeader, out IEnumerable<string> headerValues))
            {
                var asyncOperationStatusEndpoint = new Uri(headerValues.First());
                return asyncOperationStatusEndpoint.Segments.LastOrDefault();
            }
            
            return "Unknown";
        }

        private Operation ConvertToOperation(string content)
        {
            try
            {
                var operationJObject = JObject.Parse(content);
                var operation = new Operation();

                if (operationJObject.TryGetValue("Name", StringComparison.OrdinalIgnoreCase, out JToken value))
                {
                    operation.Name = (string)value;
                }

                if (operationJObject.TryGetValue("PercentComplete", StringComparison.OrdinalIgnoreCase, out value))
                {
                    operation.PercentComplete = (double)value;
                }

                if (operationJObject.TryGetValue("Status", StringComparison.OrdinalIgnoreCase, out value))
                {
                    operation.Status = (string)value;
                }

                if (operationJObject.TryGetValue("Error", StringComparison.OrdinalIgnoreCase, out value))
                {
                    operation.Error = new OperationError();
                    if (((JObject)value).TryGetValue("Code", StringComparison.OrdinalIgnoreCase, out JToken innerValue))
                    {
                        operation.Error.Code = (string)innerValue;
                    }

                    if (((JObject)value).TryGetValue("Message", StringComparison.OrdinalIgnoreCase, out innerValue))
                    {
                        operation.Error.Message = (string)innerValue;
                    }
                }

                return operation;
            }
            catch(Exception ex)
            {
                WriteDebugWithTimestamp("unable to parse operation content '{0}' exception {1}", content, ex);
                return null;
            }
        }

        private HttpRequestMessage CloneAndDisposeRequest(HttpRequestMessage original, Uri requestUri = null, HttpMethod method = null)
        {
            using (original)
            {
                var clone = new HttpRequestMessage
                {
                    Method = method ?? original.Method,
                    RequestUri = requestUri ?? original.RequestUri,
                    Version = original.Version,
                };

                foreach (KeyValuePair<string, object> prop in original.Properties)
                {
                    clone.Properties.Add(prop);
                }

                foreach (KeyValuePair<string, IEnumerable<string>> header in original.Headers)
                {
                    clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                return clone;
            }
        }

        /* protected ArmCollection GetManagedResourceCollection(ResourceIdentifier resourceId)
         {
             ServiceFabricManagedClusterResource serviceFabricManagedCluster;
             ServiceFabricManagedNodeTypeResource serviceFabricManagedNodetype;
             ServiceFabricManagedApplicationResource serviceFabricManagedApplication;
             ServiceFabricManagedApplicationTypeResource serviceFabricManagedApplicationType;
             ServiceFabricManagedApplicationTypeVersionResource serviceFabricManagedApplicationTypeVersion;
             ServiceFabricManagedServiceResource serviceFabricManagedService;

             switch (resourceId.ResourceType)
             {
                 case ClusterResource:
                     serviceFabricManagedCluster = this.ArmClient.GetServiceFabricManagedClusterResource(resourceId);
                     return serviceFabricManagedCluster.GetServiceFabricManagedApplication();

                 case NodeTypeResource:
                     break;

                 case ApplicationResource:
                     break;

                 case ApplicationTypeResource:
                     break;

                 case ApplicationTypeVersionResource:
                     break;

                 case ServiceResource:
                     break;

                 default:
                     WriteError(new ErrorRecord(new InvalidOperationException($"Invalid ResourceId: '{resourceId}'"),
                             "ResourceDoesNotExist", ErrorCategory.InvalidOperation, null));
                     break;
             }


             ResourceIdentifier serviceFabricManagedClusterResourceId = ServiceFabricManagedClusterResource.CreateResourceIdentifier(this.DefaultContext.Subscription.Id, this.ResourceGroupName, this.ClusterName);
             ServiceFabricManagedClusterResource serviceFabricManagedCluster = this.ArmClient.GetServiceFabricManagedClusterResource(serviceFabricManagedClusterResourceId);

             // get the collection of this ServiceFabricManagedApplicationTypeResource
             ServiceFabricManagedApplicationCollection collection = serviceFabricManagedCluster.GetServiceFabricManagedApplication();

             return collection;
         }*/

        protected ServiceFabricManagedClusterResource GetManagedClusterResource(string resourceGroup, string clusterName)
        {
            ResourceIdentifier serviceFabricManagedClusterResourceId = ServiceFabricManagedClusterResource.CreateResourceIdentifier(
                             this.DefaultContext.Subscription.Id,
                             resourceGroup,
                             clusterName);

            ServiceFabricManagedClusterResource serviceFabricManagedCluster = this.ArmClient.GetServiceFabricManagedClusterResource(serviceFabricManagedClusterResourceId);
            return serviceFabricManagedCluster;
        }

        protected ServiceFabricManagedClusterCollection GetServiceFabricManagedClusterCollection(string resourceGroupName)
        {

            ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(
                this.DefaultContext.Subscription.Id,
                resourceGroupName);

            ResourceGroupResource resourceGroupResource = this.ArmClient.GetResourceGroupResource(resourceGroupResourceId);

            // get the collection of this ServiceFabricManagedClusterResource
            ServiceFabricManagedClusterCollection collection = resourceGroupResource.GetServiceFabricManagedClusters();

            return collection;
        }

        #endregion
    }
}
