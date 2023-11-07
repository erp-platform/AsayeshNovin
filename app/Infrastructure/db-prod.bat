@echo off
SET ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update
SET ASPNETCORE_ENVIRONMENT=Development