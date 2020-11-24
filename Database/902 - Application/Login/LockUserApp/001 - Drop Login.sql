USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'LockUserApp')
    BEGIN
        DROP LOGIN [LockUserApp]
    END
GO