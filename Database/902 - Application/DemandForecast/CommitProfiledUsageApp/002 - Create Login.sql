USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitProfiledUsageApp')
    BEGIN
       CREATE LOGIN [CommitProfiledUsageApp] WITH PASSWORD=N'vebvSwek7m7KUrWW', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
