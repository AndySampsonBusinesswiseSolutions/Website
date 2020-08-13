USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractToSupplierData.api')
    BEGIN
        DROP LOGIN [CommitContractToSupplierData.api]
    END
GO
