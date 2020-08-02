USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFlexTradeData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempFlexTradeData.api]
    END
GO
