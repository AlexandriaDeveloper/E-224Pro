# dotnet ef migrations add Initial -s ./API -p ./Infrastructure -c ApplicationDbContext -o ./Data/Migrations


# dotnet ef database update -s ./API -p ./Infrastructure -c ApplicationDbContext 


#ng build --configuration production

#dotnet ef migrations script -o init.sql -s api -p Infrastructure -c ApplicationDbContext

# CREATE LOGIN [IIS APPPOOL\Seagull] FROM WINDOWS;


# USE [E-224Pro];
# CREATE USER [IIS APPPOOL\Seagull] FOR LOGIN [IIS APPPOOL\Seagull];
# ALTER ROLE db_datareader ADD MEMBER [IIS APPPOOL\Seagull];
# ALTER ROLE db_datawriter ADD MEMBER [IIS APPPOOL\Seagull];