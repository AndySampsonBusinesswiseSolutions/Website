USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSiteDataApp')
    BEGIN
        DROP LOGIN [StoreSiteDataApp]
    END
GO
