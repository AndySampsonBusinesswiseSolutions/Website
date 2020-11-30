USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSiteData.api')
    BEGIN
        DROP LOGIN [CommitSiteData.api]
    END
GO
