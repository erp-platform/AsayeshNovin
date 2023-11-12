# AsayeshNovin

The web service of the website of the company of "Asayesh Novin Salamat Pasargad", which is being developed based on the
ERP platform.

## Deployment

The program is deployed with the CI/CD service Liara.

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
