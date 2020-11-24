USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerToSiteDataApp')
    BEGIN
        DROP LOGIN [CommitCustomerToSiteDataApp]
    END
GO
