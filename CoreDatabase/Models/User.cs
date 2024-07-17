namespace CoreDatabase.Models;

using NpgsqlTypes;

public class User
{
    // EF Core can use hidden ctors?
    private User()
    {

    }

    public User(string name)
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public NpgsqlTsVector SearchVector { get; set; }

    public IList<Message> Messages { get; set; } = [];
    public int MessageCount { get; set; }
}
