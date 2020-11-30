USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSiteDataApp')
    BEGIN
        DROP LOGIN [ValidateSiteDataApp]
    END
GO
