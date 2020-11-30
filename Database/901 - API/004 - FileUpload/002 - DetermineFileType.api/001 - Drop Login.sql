USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'DetermineFileType.api')
    BEGIN
        DROP LOGIN [DetermineFileType.api]
    END
GO
