namespace Dexter.FromAlt;

// ~/question-sets
// ~/question-sets/123
// POST ~/question-sets/123/exercise
// req:  { question: "Why does salesforce suck?" }
// resp:
// {
//   "question": "Why does sales force suck",
//   "question_set_selected": "<GUID> / Name",
//   "question_set_candidates": "<GUID> / Name",
//   "system_prompt": "",
//   "response": ""
// }
/// <summary>
/// The question set
/// </summary>
public class QuestionSet
{
    /// <summary>
    /// ctor
    /// </summary>
    public QuestionSet(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Examples = [];
        SystemPrompt = "";
    }

    /// <summary>
    /// DB ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the set
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Examples of this question
    /// </summary>
    public List<QuestionExamples> Examples { get; set; }
    // release state (alpha, beta, rtm, etc)

    // context loaders

    // Handlebars Template?

    /// <summary>
    /// The system prompt to use in this scenario
    /// </summary>
    public string SystemPrompt { get; set; }
}

/// <summary>
/// Loads the context with more data
/// </summary>
public interface IContextLoader
{
    /// <summary>
    /// Key of the context loader
    /// </summary>
    string Key { get; }

    // Task LoadContext(InferenceContext context, CancellationToken ct = default);
}
