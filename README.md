# AsayeshNovin

The web service of the website of the company of "Asayesh Novin Salamat Pasargad", which is being developed based on the
ERP platform.

## Deployment

The program is deployed with the CI/CD service Liara. To create a new migration and update the database, use the .bat
files located in app/Infrastructure path. First, execute the db-dev.bat file. This file runs Entity Framework Core
commands locally. The migration name should change with each execution of this file. To update the database on the
server, run the db-prod.bat file.

### DB Migrations

#### Install EF Core Tools

```shell
dotnet tool install --global dotnet-ef
```

### Update

```shell
dotnet ef migrations add MigrationName
dotnet ef database update
```