USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateQuarterForecast.api')
    BEGIN
       CREATE LOGIN [CreateQuarterForecast.api] WITH PASSWORD=N'J6qwzGCxU7ERtjwy', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
