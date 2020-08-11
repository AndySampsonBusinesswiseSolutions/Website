USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerToSiteData.api')
    BEGIN
        DROP LOGIN [CommitCustomerToSiteData.api]
    END
GO
