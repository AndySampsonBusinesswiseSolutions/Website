USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToSiteData.api')
    BEGIN
        DROP LOGIN [CommitMeterToSiteData.api]
    END
GO
