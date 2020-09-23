USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapFutureDateIdToUsageDateIdByDate.api')
    BEGIN
        DROP LOGIN [MapFutureDateIdToUsageDateIdByDate.api]
    END
GO
