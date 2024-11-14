namespace Dexter.FromAlt;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// DB Context for Inference
/// </summary>
public class InferenceDbContext: DbContext
{
    /// <summary>
    /// Question Sets
    /// </summary>
    public DbSet<QuestionSet> QuestionSets { get; } = null!;

    /// <summary>
    /// Question Examples
    /// </summary>
    public DbSet<QuestionExamples> QuestionExamples { get; } = null!;
}
