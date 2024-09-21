# Backup in Image kopieren
docker cp ./backup/AIChecker.bak  MyMS_SQLServer:AIChecker.bak
# alte DB entfernen
docker exec -it 'MyMS_SQLServer' /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '123456789!_Asdf' -C -Q 'ALTER DATABASE [AIChecker] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
drop database [AIChecker]
GO'
# neue DB importieren
docker exec -it 'MyMS_SQLServer' /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '123456789!_Asdf' -C -Q "RESTORE DATABASE [AIChecker] FROM  DISK = N'/AIChecker.bak' WITH  FILE = 1,  NOUNLOAD,  STATS = 5"
# User sa auf Deutsch setzen für Datumskonvertierung
#docker exec -it 'MyMS_SQLServer' /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '123456789!_Asdf' -Q "ALTER LOGIN [sa] WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[Deutsch], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON"
