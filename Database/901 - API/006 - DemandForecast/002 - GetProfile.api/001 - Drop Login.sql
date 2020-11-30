USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetProfile.api')
    BEGIN
        DROP LOGIN [GetProfile.api]
    END
GO
