# Basis Theory .NET Sample App

Example of creating a search endpoint using Lucene and entity framework.

This code is referenced by the How to transform a search query into an Entity Framework expression with Lucene? blog post.

## Dependencies
- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

Verify .NET is installed by running: `dotnet --list-sdks` and verify an entry with version `6.*` is displayed

## Run the app

```
make search
```

The application will launch on port http://localhost:5082/search and you can hit it as
a POST endpoint that accepts JSON body such as:

```
    {
        "query": "firstName:Bob"
    }
```