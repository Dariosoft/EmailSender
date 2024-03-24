@echo off
cls
SET ver=1.0.1
SET sln=D:\Dariosoft\Projects\2024\EmailSender
SET prj=%sln%\Project\SDK\gRPC.SDK\Dariosoft.EmailSender.EndPoint.gRPC.SDK.csproj
SET pkg=%sln%\Project\SDK\gRPC.SDK\bin\Release\Dariosoft.EmailSender.EndPoint.gRPC.SDK.%ver%.nupkg

dotnet build -property:SolutionDir=%sln%  %prj% -c Release
dotnet pack %prj%
dotnet nuget push %pkg% -s https://nuget.hostapp.org/nuget -k 48wzKTVC2YreF7RS8H6mKjQsaOGiXDDjbe4EZhKR
