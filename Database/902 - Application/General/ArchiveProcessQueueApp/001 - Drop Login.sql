USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ArchiveProcessQueueApp')
    BEGIN
        DROP LOGIN [ArchiveProcessQueueApp]
    END
GO