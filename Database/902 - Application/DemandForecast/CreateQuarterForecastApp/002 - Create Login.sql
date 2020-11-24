USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateQuarterForecastApp')
    BEGIN
       CREATE LOGIN [CreateQuarterForecastApp] WITH PASSWORD=N'J6qwzGCxU7ERtjwy', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
