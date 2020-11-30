USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'DataAnalysisWebpageGetForecast.api')
    BEGIN
       CREATE LOGIN [DataAnalysisWebpageGetForecast.api] WITH PASSWORD=N'nJbgPkmV6JFh26GS', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
