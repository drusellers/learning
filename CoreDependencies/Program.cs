using CoreDependencies;



var services = new ServiceCollection();


services.AddScoped<object>(provider =>
{
    return ActivatorUtilities.CreateInstance<object>(provider, )
});

services.AddKeyedTransient<INamedService, BobService>("bob");
services.AddKeyedTransient<INamedService, BillService>("bill");

#pragma warning disable ASP0000
var provider = services.BuildServiceProvider(true);
#pragma warning restore ASP0000

// What happens if we don't use a name? (InvalidOperationException)
// var x = provider.GetRequiredService<INamedService>();

var x = provider.GetRequiredKeyedService<INamedService>("bob");
