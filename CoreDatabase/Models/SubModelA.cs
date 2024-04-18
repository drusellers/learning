namespace CoreDatabase.Models;

public interface ICommonModel
{
    public string Name { get;  }
}

public class SubModelA: ICommonModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class SubModelB: ICommonModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
