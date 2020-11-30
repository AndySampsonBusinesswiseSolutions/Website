USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitGridSupplyPointToMeterData.api')
    BEGIN
        DROP LOGIN [CommitGridSupplyPointToMeterData.api]
    END
GO
