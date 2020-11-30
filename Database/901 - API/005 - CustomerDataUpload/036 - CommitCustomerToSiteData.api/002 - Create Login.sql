USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerToSiteData.api')
    BEGIN
       CREATE LOGIN [CommitCustomerToSiteData.api] WITH PASSWORD=N'4XWtk5BbBFkNf34d', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
