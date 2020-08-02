USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFlexTradeData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempFlexTradeData.api]
    END
GO
