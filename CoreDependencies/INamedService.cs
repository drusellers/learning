namespace CoreDependencies;

public interface INamedService
{
    string Chirp();
}

public class BobService : INamedService
{
    public string Chirp()
    {
        return "Bob";
    }
}

public class BillService : INamedService
{
    public string Chirp()
    {
        return "Bill";
    }
}
