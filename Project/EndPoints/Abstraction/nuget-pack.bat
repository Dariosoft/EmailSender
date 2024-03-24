@echo off
cls
SET ver=2.0.0
SET sln=D:\Dariosoft\Projects\2024\EmailSender
SET prj=%sln%\Project\EndPoints\Abstraction\Dariosoft.EmailSender.EndPoint.Abstraction.csproj
SET pkg=%sln%\Project\EndPoints\Abstraction\bin\Release\Dariosoft.EmailSender.EndPoint.Abstraction.%ver%.nupkg

dotnet build -property:SolutionDir=%sln%  %prj% -c Release
dotnet pack %prj%
dotnet nuget push %pkg% -s https://nuget.hostapp.org/nuget -k 48wzKTVC2YreF7RS8H6mKjQsaOGiXDDjbe4EZhKR
