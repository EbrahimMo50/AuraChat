## Github

1. commit naming will follow
    (type)(optional scope): <description>
    
    **##example**

          Create: authorization middleware

2. branch naming wil follow
    (objective)/(name of the branch)
    
    **##example**

          new/authorization-system

## Code Base
##### Refer to the default coding conventions <a href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#naming-conventions">.NET Identifier Names</a>
**on top of default coding conventions we will follow:**

1. namespace to be file-scoped
2. XML commenting on complex methods
3. Don't capitalize abbreviations (use `UserGuid` instead of `UserGUID`)
4. async methods must be followed by async so callers identify the method is async like `GetUsersAsync`
