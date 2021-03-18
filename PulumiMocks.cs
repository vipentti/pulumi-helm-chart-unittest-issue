using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi.Testing;

namespace pulumi_helm_chart_unittest_example
{
    public delegate void ConfigureResourcesAction(string type, string name, ImmutableDictionary<string, object> inputs, ImmutableDictionary<string, object>.Builder outputs);

    public class PulumiMocks : IMocks
    {
        private readonly ConfigureResourcesAction? _configureMocks;

        public PulumiMocks(ConfigureResourcesAction? configureMocks = default)
        {
            _configureMocks = configureMocks;
        }

        public Task<object> CallAsync(string token, ImmutableDictionary<string, object> args, string? provider)
        {

	    var outputs = ImmutableDictionary.CreateBuilder<string, object>();
	    outputs.AddRange(args);

	    if (token == "kubernetes:helm:template") {
		outputs.Add("result", ImmutableArray.Create<object>());
	    }

            return Task.FromResult<object>(outputs.ToImmutable());
        }

        public Task<(string? id, object state)> NewResourceAsync(string type, string name, ImmutableDictionary<string, object> inputs, string? provider, string? id)
        {
            ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();

            // Forward all input parameters as resource outputs, so that we could test them.
            outputs.AddRange(inputs);

            _configureMocks?.Invoke(type, name, inputs, outputs);

            // Default the resource ID to `{name}_id`.
            id ??= $"{name}_id";

            return Task.FromResult<(string?, object)>((id, outputs));
        }
    }

}
