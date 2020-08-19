USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitPeriodicUsageData.api')
    BEGIN
       CREATE LOGIN [CommitPeriodicUsageData.api] WITH PASSWORD=N'8pSATRRftgLbJ5Zm', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
