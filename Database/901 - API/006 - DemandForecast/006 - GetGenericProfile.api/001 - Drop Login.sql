USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetGenericProfile.api')
    BEGIN
        DROP LOGIN [GetGenericProfile.api]
    END
GO
