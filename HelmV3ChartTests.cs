using System;
using Xunit;
using System.Threading.Tasks;
using Pulumi;
using FluentAssertions;

namespace pulumi_helm_chart_unittest_example
{
    public class HelmV3Stack : Stack
    {
        public HelmV3Stack()
        {
            var chart = new Pulumi.Kubernetes.Helm.V3.Chart("chart"
                , new Pulumi.Kubernetes.Helm.ChartArgs()
                {
                    Chart = "ingress-nginx",
                    Namespace = "kube-system",
                    FetchOptions = new Pulumi.Kubernetes.Helm.ChartFetchArgs
                    {
                        Repo = "https://kubernetes.github.io/ingress-nginx",
                    },
                });
        }
    }

    public class HelmV3ChartTests
    {
        [Fact]
        public async Task Should_Create_Resources()
        {
            var resources = await Testing.RunAsync<HelmV3Stack>(new PulumiMocks());

            resources.Should().NotBeEmpty();
        }
    }
}
