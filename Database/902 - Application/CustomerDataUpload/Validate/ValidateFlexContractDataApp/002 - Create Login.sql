USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexContractDataApp')
    BEGIN
       CREATE LOGIN [ValidateFlexContractDataApp] WITH PASSWORD=N'FMTXVhfa5Yu8s6vQ', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
