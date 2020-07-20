USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFlexContractData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempFlexContractData.api] WITH PASSWORD=N'W92dpvtPzz3uJfYg', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
