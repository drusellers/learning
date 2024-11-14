namespace Dexter.FromAlt;

using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

/// <summary>
/// https://github.com/pgvector/pgvector-dotnet
/// https://github.com/pgvector/pgvector-dotnet?tab=readme-ov-file#entity-framework-core
/// </summary>
public class QuestionSetRepository
{
    readonly InferenceDbContext _context;
    readonly IQueryable<QuestionSet> _baseQuery;
    readonly IInferenceEngine _inference;


    /// <summary>
    /// ctor
    /// </summary>
    public QuestionSetRepository(InferenceDbContext context, IInferenceEngine inference)
    {
        _context = context;
        _inference = inference;
        _baseQuery = _context.QuestionSets
            .Include(x => x.Examples);
    }

    /// <summary>
    /// If we can't find a match - what should we prompt with?
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<QuestionSet> DefaultQuestionSet(CancellationToken ct = default)
    {
        return _baseQuery.SingleAsync(x => x.Name == "Default", ct);
    }

    /// <summary>
    /// Finds the nearest question set
    /// </summary>
    public Task<QuestionSet?> FindNearestQuestionSet(string question, CancellationToken ct = default)
    {
        var embedding = new Vector("ABC");

        return FindNearestQuestionSet(embedding, ct);
    }

    /// <summary>
    /// Finds the nearest question set
    /// </summary>
    public async Task<QuestionSet?> FindNearestQuestionSet(Vector question, CancellationToken ct = default)
    {
        var answer = await _context.QuestionExamples.OrderBy(x => x.Embedding!.L2Distance(question))
            .Include(x => x.QuestionSet)
            .Take(1)
            .FirstOrDefaultAsync(cancellationToken: ct);

        return answer?.QuestionSet;
    }

    // Getting a distance
    // var items = await ctx.Items
    // .Select(x => new { Entity = x, Distance = x.Embedding!.L2Distance(embedding) })
    // .ToListAsync();

    /// <summary>
    /// Vectorize a specific set
    /// </summary>
    public Task VectorizeQuestionSet(Guid id, CancellationToken ct = default)
    {
        // should I error if its already vectorized?
        throw new NotImplementedException("Not yet ready");
    }

    /// <summary>
    /// Find all questions set examples and vectorize missing items.
    /// </summary>
    /// <param name="ct"></param>
    public async Task VectorizeQuestionSets(CancellationToken ct = default)
    {
        var examples = await _context.QuestionExamples.Where(x => x.Embedding == null)
            .ToListAsync(ct);

        foreach (var example in examples)
        {
            var x = await _inference.GetEmbedding(example.Example, ct);
            example.Embedding = new Vector(x);
            _context.Update(example);
        }

        await _context.SaveChangesAsync(ct);
    }
}
