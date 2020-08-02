USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexContractData.api')
    BEGIN
        DROP LOGIN [StoreFlexContractData.api]
    END
GO
