USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempMeterData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempMeterData.api] WITH PASSWORD=N'TqaV8u53zBrSksr4', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
