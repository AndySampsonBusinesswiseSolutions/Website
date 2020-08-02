USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFlexContractData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempFlexContractData.api]
    END
GO
