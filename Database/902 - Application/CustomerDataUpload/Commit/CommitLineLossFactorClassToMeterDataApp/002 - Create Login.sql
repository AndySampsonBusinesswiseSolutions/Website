USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitLineLossFactorClassToMeterDataApp')
    BEGIN
       CREATE LOGIN [CommitLineLossFactorClassToMeterDataApp] WITH PASSWORD=N'BUJXnVrsxzCBEX58', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
