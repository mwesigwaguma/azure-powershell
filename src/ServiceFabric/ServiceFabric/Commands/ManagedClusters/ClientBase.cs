#region Assembly Azure.Core.TestFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6a281e58f0dfecb7
// D:\repos\azure-sdk-for-net\artifacts\obj\Azure.Core.TestFramework\Debug\net6.0\ref\Azure.Core.TestFramework.dll
#endregion

using Castle.DynamicProxy;
//using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    public abstract class ClientTestBase
    {
        public ClientTestBase(bool isAsync);

        public bool IsAsync { get; }
        public bool TestDiagnostics { get; set; }
        protected IReadOnlyCollection<IInterceptor> AdditionalInterceptors { get; set; }
        protected virtual DateTime TestStartTime { get; }

        protected TClient InstrumentClient<TClient>(TClient client, IEnumerable<IInterceptor> preInterceptors) where TClient : class;
    }
}