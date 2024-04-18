namespace CoreDatabase.Models;

public class Message
{
    public int Id { get; set; }
    public User User { get; set; }
    public string Payload { get; set; }
}
