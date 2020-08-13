USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToTimeswitchCodeData.api')
    BEGIN
       CREATE LOGIN [CommitMeterToTimeswitchCodeData.api] WITH PASSWORD=N'7wSWYFFtFGr6qETP', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
