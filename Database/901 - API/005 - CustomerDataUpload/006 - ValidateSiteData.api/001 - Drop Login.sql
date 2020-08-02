USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSiteData.api')
    BEGIN
        DROP LOGIN [ValidateSiteData.api]
    END
GO
