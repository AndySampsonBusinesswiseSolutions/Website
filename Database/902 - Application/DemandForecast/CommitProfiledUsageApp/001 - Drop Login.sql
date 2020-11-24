USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitProfiledUsageApp')
    BEGIN
        DROP LOGIN [CommitProfiledUsageApp]
    END
GO
