@echo off
cls
SET ver=1.0.0
SET sln=D:\Dariosoft\Projects\2024\EmailSender
SET prj=%sln%\Core\Enums\Dariosoft.EmailSender.Enums.csproj
SET pkg=%sln%\Core\Enums\bin\Release\Dariosoft.EmailSender.Enums.%ver%.nupkg

dotnet build -property:SolutionDir=%sln%  %prj% -c Release
dotnet pack %prj%
dotnet nuget push %pkg% -s https://nuget.hostapp.org/nuget -k 48wzKTVC2YreF7RS8H6mKjQsaOGiXDDjbe4EZhKR
