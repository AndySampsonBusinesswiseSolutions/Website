USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Routing.api')
    BEGIN
        DROP LOGIN [Routing.api]
    END
GO