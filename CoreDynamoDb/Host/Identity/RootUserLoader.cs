namespace Id.Host.Identity;

using Microsoft.Extensions.Hosting;
using Queries;

public class RootUserLoader : IHostedService
{
    readonly DynamoContext _context;

    public RootUserLoader(DynamoContext context)
    {
        _context = context;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _context.ResetTable();

        var user1 = new User
        {
            ProviderType = "BASIC",
            ProviderId = "root",
            EmailAddress = "root@twosix.io",
        };
        var user2 = new User
        {
            ProviderType = "BASIC",
            ProviderId = "bob",
            EmailAddress = "bob@twosix.io"
        };
        await _context.PutUser(user1);
        await _context.PutUser(user1);
        await _context.PutUser(user2);
        await _context.PutAccount(new Account
        {
            AccountId = "ABC"
        });
        await _context.PutAccount(new Account
        {
            AccountId = "DEF"
        });

        await _context.PutMembership(new Membership
        {
            User = user1,
            Account = "ABC",
            Role = "Owner"
        });
        await _context.PutMembership(new Membership
        {
            User = user1,
            Account = "DEF",
            Role = "Owner"
        });
        await _context.PutMembership(new Membership
        {
            User = user2,
            Account = "DEF",
            Role = "Member"
        });
        await _context.PutAdmin(new AdminRole
        {
            User = user1,
            Role = "RW"
        });

        await new MembershipsByUser(user1).Execute(_context);
        await new UserByProviderCredentials("BASIC","root").Execute(_context);

        Console.WriteLine("DONE");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
