USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateId.api')
    BEGIN
        DROP LOGIN [MapFutureDateIdToUsageDateId.api]
    END
GO
