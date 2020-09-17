USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateForecastUsage.api')
    BEGIN
       CREATE LOGIN [CreateForecastUsage.api] WITH PASSWORD=N'uN9pHEnxyfsKxSVJ', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
