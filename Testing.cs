using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Testing;

namespace pulumi_helm_chart_unittest_example
{
    public static class Testing
    {
        public static void WithConfig(Dictionary<string, object> config)
        {
            var values = System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions()
            {
            });

            Environment.SetEnvironmentVariable("PULUMI_CONFIG", values);
        }

        public static Task<ImmutableArray<Resource>> RunAsync<T>(PulumiMocks mocks, string? project = default, string? stack = default)
            where T : Stack, new()
        {
            project ??= "project";
            stack ??= $"{project}.stack";


            return Deployment.TestAsync<T>(mocks, new TestOptions { IsPreview = false, ProjectName = project, StackName = stack });
        }

        /// <summary>
        /// Extract the value from an output.
        /// </summary>
        public static Task<T> GetValueAsync<T>(this Output<T> output)
        {
            var tcs = new TaskCompletionSource<T>();
            output.Apply(v =>
            {
                tcs.SetResult(v);
                return v;
            });
            return tcs.Task;
        }
    }
}
