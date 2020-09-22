USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetMappedUsageDateId.api')
    BEGIN
        DROP LOGIN [GetMappedUsageDateId.api]
    END
GO
