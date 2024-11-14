namespace Dexter.FromAlt.Providers.OpenAi;

using Microsoft.Extensions.Options;
using OpenAI;

/// <summary>
/// Bind to the Open AI Model
/// </summary>
public class OpenAiInferenceEngine : IInferenceEngine
{
    readonly IOptions<InferenceOptions> _options;

    /// <summary>
    /// ctor
    /// </summary>
    public OpenAiInferenceEngine(IOptions<InferenceOptions> options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<ReadOnlyMemory<float>> GetEmbedding(string input, CancellationToken ct = default)
    {
        var openAi = new OpenAIClient(_options.Value.OpenAi!.ApiKey);
        var emb = await openAi.EmbeddingsEndpoint.CreateEmbeddingAsync(input, null, null, null, ct);

        if (emb.Data.Count > 1)
        {
            throw new Exception("WAT:Embeddings");
        }

        var a = emb.Data.First().Embedding
            .Select(x => (float)x)
            .ToArray();

        return new ReadOnlyMemory<float>(a);
    }
}
