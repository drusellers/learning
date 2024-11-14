namespace Dexter.FromAlt;

/// <summary>
/// The inference engine in use
/// </summary>
public interface IInferenceEngine
{
    /// <summary>
    /// Generate Embeddings
    /// </summary>
    Task<ReadOnlyMemory<float>> GetEmbedding(string input, CancellationToken ct = default);
}
