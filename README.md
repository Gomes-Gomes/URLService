# URL SERVICE

## TargetFramework net5.0

## USE

````powershell
PS C:\URLService> dotnet build
Microsoft (R) Build Engine version 16.10.2+857e5a733 for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  All projects are up-to-date for restore.
  URLService -> C:\URLService\bin\Debug\net5.0\URLService.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:04.19
PS C:\URLService> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\URLService


````

Go To: <https://localhost:5001/swagger/index.html>

Let´s Setup swagger:

- To serve the Swagger UI at the app's roots set the RoutePrefix property to an empty string:

- Add Swagger Info and xml documentation

````C#
   c.DocumentFilter<JsonPatchDocumentFilter>();

                foreach (var filePath in System.IO.Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
                {
                    try
                    {
                        c.IncludeXmlComments(filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

````

- Add Custom css dark mode to swagger

- Add custom logo in swagger based on github org <https://api.github.com/orgs/Gomes-Gomes> ... let´s see if whe we change the avatar the logo is gone

````CSS
img[alt="Swagger UI"] {
    display: block;
    -moz-box-sizing: border-box;
    box-sizing: border-box;
    content: url('https://avatars.githubusercontent.com/u/99501804?v=4');
    max-width: 100%;
    max-height: 100%;
}


````

````bash
git init
git add README.md
git commit -m "first commit"
git branch -M main
git remote add origin https://github.com/Gomes-Gomes/URLService.git
git push -u origin main

````
