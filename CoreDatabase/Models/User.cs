namespace CoreDatabase.Models;

using NpgsqlTypes;

public class User
{
    // EF Core can use hidden ctors?
    private User()
    {
        int i = 0;
    }

    public User(string name)
    {
        Name = name;
        SearchVector = NpgsqlTsVector.Parse(name);
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public NpgsqlTsVector SearchVector { get; set; }

    public IList<Message> Messages { get; set; } = new List<Message>();
}
