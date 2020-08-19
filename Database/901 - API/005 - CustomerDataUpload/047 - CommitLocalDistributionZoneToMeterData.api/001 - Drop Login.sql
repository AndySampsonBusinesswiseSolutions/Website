USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitLocalDistributionZoneToMeterData.api')
    BEGIN
        DROP LOGIN [CommitLocalDistributionZoneToMeterData.api]
    END
GO
