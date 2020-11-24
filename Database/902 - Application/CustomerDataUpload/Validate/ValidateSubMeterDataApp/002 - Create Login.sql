USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterDataApp')
    BEGIN
       CREATE LOGIN [ValidateSubMeterDataApp] WITH PASSWORD=N'nqCLyMb92urhKraj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
