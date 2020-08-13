USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractMeterToProductData.api')
    BEGIN
        DROP LOGIN [CommitContractMeterToProductData.api]
    END
GO
