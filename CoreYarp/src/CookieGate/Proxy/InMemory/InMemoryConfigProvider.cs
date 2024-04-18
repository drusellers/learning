namespace CookieGate.Proxy.InMemory;

using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

public class InMemoryConfigProvider : IProxyConfigProvider
{
    volatile InMemoryConfig _config;

    public InMemoryConfigProvider(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        _config = new InMemoryConfig(routes, clusters);
    }

    public IProxyConfig GetConfig()
    {
        return _config;
    }

    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var oldConfig = _config;
        _config = new InMemoryConfig(routes, clusters);
        oldConfig.SignalChange();
    }


    class InMemoryConfig : IProxyConfig
    {
        readonly CancellationTokenSource _cts = new();

        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken { get; }

        internal void SignalChange()
        {
            _cts.Cancel();
        }
    }
}
