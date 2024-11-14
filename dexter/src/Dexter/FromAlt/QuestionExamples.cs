namespace Dexter.FromAlt;

using System.Diagnostics;
using Pgvector;

/// <summary>
/// An example question and its associated vector
/// </summary>
[DebuggerDisplay("{Example}")]
public class QuestionExamples
{
    /// <summary>
    /// ctor
    /// </summary>
    public QuestionExamples(QuestionSet questionSet, string exampleQuestion)
    {
        Id = Guid.NewGuid();
        Example = exampleQuestion;
        QuestionSet = questionSet;
    }

    /// <summary>
    /// DB ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// What question set does it belong to?
    /// </summary>
    public QuestionSet QuestionSet { get; set; }

    /// <summary>
    /// Example Question Text
    /// </summary>
    public string Example { get; set; }

    /// <summary>
    /// The vector data
    /// </summary>
    public Vector? Embedding { get; set; }
}
