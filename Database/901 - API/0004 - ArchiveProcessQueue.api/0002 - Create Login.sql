USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ArchiveProcessQueue.api')
    BEGIN
        CREATE LOGIN [ArchiveProcessQueue.api] WITH PASSWORD=N'nb@89qWEW5!6=2s*', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO