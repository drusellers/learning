namespace CookieGate.Proxy;

using Yarp.ReverseProxy.Configuration;

public static class ProxyConfiguration
{
    public static RouteConfig[] GetRoutes()
    {
        return new[]
        {
            new RouteConfig
            {
                RouteId = "logged-in",
                ClusterId = "logged-in",
                Match = new RouteMatch
                {
                    Path = "/{**slop}/",
                    Headers = new []
                    {
                        new RouteHeader
                        {
                            Mode = HeaderMatchMode.Contains,
                            Name = "Cookie",
                            Values = new []
                            {
                                "MultiPass"
                            }
                        }
                    }
                }
            },
            new RouteConfig
            {
                RouteId = "logged-out",
                ClusterId = "logged-out",
                Match = new RouteMatch
                {
                    Path = "/{**slop}/",
                }
            },
        };
    }

    public static ClusterConfig[] GetClusters()
    {
        return new[]
        {
            new ClusterConfig
            {
                ClusterId = "logged-in",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        "des1", new DestinationConfig
                        {
                            Address = "http://localhost:8080"
                        }

                    }
                }
            },
            new ClusterConfig
            {
                ClusterId = "logged-out",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "des2", new DestinationConfig { Address = "http://localhost:8081" } }
                }
            }
        };
    }
}
