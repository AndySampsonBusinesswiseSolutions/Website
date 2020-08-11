USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubMeterUsageData.api')
    BEGIN
       CREATE LOGIN [CommitSubMeterUsageData.api] WITH PASSWORD=N'BahJpvV8hyTwC3Bt', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
