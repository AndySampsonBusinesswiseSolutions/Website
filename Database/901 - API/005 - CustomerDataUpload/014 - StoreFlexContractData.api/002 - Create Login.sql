USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexContractData.api')
    BEGIN
       CREATE LOGIN [StoreFlexContractData.api] WITH PASSWORD=N'W92dpvtPzz3uJfYg', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
