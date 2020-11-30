USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateFiveMinuteForecastApp')
    BEGIN
       CREATE LOGIN [CreateFiveMinuteForecastApp] WITH PASSWORD=N'zTqVewH8Zrgye4Vd', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
