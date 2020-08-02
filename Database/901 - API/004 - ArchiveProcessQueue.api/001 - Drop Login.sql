USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ArchiveProcessQueue.api')
    BEGIN
        DROP LOGIN [ArchiveProcessQueue.api]
    END
GO