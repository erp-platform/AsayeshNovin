@echo off
SET ASPNETCORE_ENVIRONMENT=Development
dotnet ef migrations add init
dotnet ef database update