USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFlexTradeData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempFlexTradeData.api] WITH PASSWORD=N'89zfZ2GTajb4B94y', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
