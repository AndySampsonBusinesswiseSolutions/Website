USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerDataApp')
    BEGIN
       CREATE LOGIN [CommitCustomerDataApp] WITH PASSWORD=N'yuPPW9N2d346ATSM', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
