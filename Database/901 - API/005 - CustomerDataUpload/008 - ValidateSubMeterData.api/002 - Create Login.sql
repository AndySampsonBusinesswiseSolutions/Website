USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterData.api')
    BEGIN
       CREATE LOGIN [ValidateSubMeterData.api] WITH PASSWORD=N'nqCLyMb92urhKraj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
