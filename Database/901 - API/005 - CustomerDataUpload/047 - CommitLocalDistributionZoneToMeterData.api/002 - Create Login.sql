USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitLocalDistributionZoneToMeterData.api')
    BEGIN
       CREATE LOGIN [CommitLocalDistributionZoneToMeterData.api] WITH PASSWORD=N'JT4paBW9eH4m6F35', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
