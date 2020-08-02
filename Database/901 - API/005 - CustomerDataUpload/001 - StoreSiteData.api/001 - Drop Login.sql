USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSiteData.api')
    BEGIN
        DROP LOGIN [StoreSiteData.api]
    END
GO
