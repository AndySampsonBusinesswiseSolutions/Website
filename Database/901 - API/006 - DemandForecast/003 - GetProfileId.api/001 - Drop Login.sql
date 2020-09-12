USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetProfileId.api')
    BEGIN
        DROP LOGIN [GetProfileId.api]
    END
GO
