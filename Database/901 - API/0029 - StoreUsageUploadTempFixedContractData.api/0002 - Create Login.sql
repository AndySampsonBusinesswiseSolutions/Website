USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFixedContractData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempFixedContractData.api] WITH PASSWORD=N'ReAjquZxWE6SrqjB', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
