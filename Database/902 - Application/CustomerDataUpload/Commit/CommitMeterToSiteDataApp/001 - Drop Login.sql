USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToSiteDataApp')
    BEGIN
        DROP LOGIN [CommitMeterToSiteDataApp]
    END
GO
