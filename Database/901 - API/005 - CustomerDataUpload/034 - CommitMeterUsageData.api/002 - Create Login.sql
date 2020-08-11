USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterUsageData.api')
    BEGIN
       CREATE LOGIN [CommitMeterUsageData.api] WITH PASSWORD=N'ad9HJxv48Px7gUTj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
