# Database EFCore Questions

- Goal: Write the least amount of code to get what you need / want

Previously I've written a "Mapping" class for each type I create. So if I have a `User` type then I will have a `UserMapping` type. This is an old behavior of mine that dates back to NHibernate and its XML model. That was then carried forward into Fluent.NHibernate. I started to work on some Fluent.NHibernate conventions, but that was more about mapping custom types than anything else.

Well now years later, I'm working on EFCore and I def have a different take on how I want mapping to be done. Combined with reviewing a friend's application where I saw a lot more conventional based mapping and I wanted to explore that a bit more.

Once you have docker running, you can create a migration with an easy reset below.

```sh
./scripts/db --create Initial --reset --hard && dotnet run
```

EFCore becomes aware when you
1. add a DbSet property
2. add a Entity Type Mapping in `OnModelCreating` 


- Map Enum as String
- Map PK's
