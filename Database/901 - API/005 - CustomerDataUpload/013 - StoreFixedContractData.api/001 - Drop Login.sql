USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFixedContractData.api')
    BEGIN
        DROP LOGIN [StoreFixedContractData.api]
    END
GO
