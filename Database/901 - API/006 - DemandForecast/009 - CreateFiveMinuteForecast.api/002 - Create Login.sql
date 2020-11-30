USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateFiveMinuteForecast.api')
    BEGIN
       CREATE LOGIN [CreateFiveMinuteForecast.api] WITH PASSWORD=N'zTqVewH8Zrgye4Vd', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
