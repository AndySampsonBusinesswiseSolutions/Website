USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDataAnalysisWebpage.api')
    BEGIN
       CREATE LOGIN [CreateDataAnalysisWebpage.api] WITH PASSWORD=N'T4Ss27brmmYtMAPN', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
