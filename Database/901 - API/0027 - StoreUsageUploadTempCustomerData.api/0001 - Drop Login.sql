USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempCustomerData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempCustomerData.api]
    END
GO
