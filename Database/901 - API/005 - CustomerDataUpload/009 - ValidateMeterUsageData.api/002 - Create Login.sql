USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterUsageData.api')
    BEGIN
       CREATE LOGIN [ValidateMeterUsageData.api] WITH PASSWORD=N'qashfvSa2xB58PXR', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
