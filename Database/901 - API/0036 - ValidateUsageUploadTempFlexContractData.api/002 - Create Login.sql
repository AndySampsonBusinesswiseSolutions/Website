USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFlexContractData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempFlexContractData.api] WITH PASSWORD=N'FMTXVhfa5Yu8s6vQ', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
