USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateHalfHourForecast.api')
    BEGIN
       CREATE LOGIN [CreateHalfHourForecast.api] WITH PASSWORD=N'EqQVsbWULSyW85bU', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
