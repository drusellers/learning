namespace Dexter.FromAlt.Providers.Claudia;

/// <summary>
/// The claude inference engine
/// </summary>
public class ClaudInferenceEngine : IInferenceEngine
{
    /// <inheritdoc />
    public Task<ReadOnlyMemory<float>> GetEmbedding(string input, CancellationToken ct = default)
    {
        // Anthropic SDK Doesn't support this
        throw new NotImplementedException();
    }
}
