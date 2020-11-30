USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitLocalDistributionZoneToMeterDataApp')
    BEGIN
        DROP LOGIN [CommitLocalDistributionZoneToMeterDataApp]
    END
GO
