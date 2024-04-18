# Cookie Gate

This demos how to have YARP proxy a different site based on the presence of a cookie or not


```sh
cd loggedIn && npx http-serve
cd loggedOut && PORT=8081 npx http-serve
dotnet run
```
