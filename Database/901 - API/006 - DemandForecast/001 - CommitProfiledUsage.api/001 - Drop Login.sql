USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitProfiledUsage.api')
    BEGIN
        DROP LOGIN [CommitProfiledUsage.api]
    END
GO
