USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitAssetToSubMeterData.api')
    BEGIN
       CREATE LOGIN [CommitAssetToSubMeterData.api] WITH PASSWORD=N'Upr4fm9NKd8mC5Dt', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
