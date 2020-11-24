USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitGridSupplyPointToMeterDataApp')
    BEGIN
        DROP LOGIN [CommitGridSupplyPointToMeterDataApp]
    END
GO
