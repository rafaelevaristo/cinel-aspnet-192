# Project MVC - CINEL stuff

## If you use VSCODE i suggest to install these extentions

#### C# Dev Kit: 
https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit 

After extention installation a minitutorial will be available.
 

#### MSSQL tools : 
https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql


### Command to list dotnet mvc template - Just for learning purposes 
``` 
dotnet new mvc --help 
```

### Create a project  - Pls dont if you are clonning ;) 
```
dotnet new mvc -n ASPNET192 --auth individual
```
#### Command explanation 
dotnet new mvc : create a new project based on MVC dotnet template

-n ASPNET192 : the project will be named ASPNET192

--auth individual : use the Individual account template option for MVC. This will add the Identity to mvc template. Check the template options using the respective command .

#### Install Entity Framework Core tools reference - .NET Core CLI
CLI EF : https://learn.microsoft.com/en-us/ef/core/cli/dotnet