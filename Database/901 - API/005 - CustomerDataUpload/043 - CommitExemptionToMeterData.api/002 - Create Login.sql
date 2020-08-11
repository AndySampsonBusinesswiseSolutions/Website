USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitExemptionToMeterData.api')
    BEGIN
       CREATE LOGIN [CommitExemptionToMeterData.api] WITH PASSWORD=N'WeJqMap8UMB7Uayq', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
