USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSiteDataApp')
    BEGIN
        DROP LOGIN [CommitSiteDataApp]
    END
GO
