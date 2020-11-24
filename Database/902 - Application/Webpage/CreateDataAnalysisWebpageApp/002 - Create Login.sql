USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDataAnalysisWebpageApp')
    BEGIN
       CREATE LOGIN [CreateDataAnalysisWebpageApp] WITH PASSWORD=N'T4Ss27brmmYtMAPN', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
