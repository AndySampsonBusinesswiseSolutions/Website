USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFixedContractDataApp')
    BEGIN
        DROP LOGIN [StoreFixedContractDataApp]
    END
GO
