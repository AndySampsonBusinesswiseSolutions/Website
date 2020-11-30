USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitProfiledUsage.api')
    BEGIN
       CREATE LOGIN [CommitProfiledUsage.api] WITH PASSWORD=N'vebvSwek7m7KUrWW', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
