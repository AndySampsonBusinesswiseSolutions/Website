USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFixedContractData.api')
    BEGIN
       CREATE LOGIN [StoreFixedContractData.api] WITH PASSWORD=N'ReAjquZxWE6SrqjB', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
