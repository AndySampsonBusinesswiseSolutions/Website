USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempMeterExempionData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempMeterExempionData.api] WITH PASSWORD=N'XdUWncBgXVs2hmvE', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
