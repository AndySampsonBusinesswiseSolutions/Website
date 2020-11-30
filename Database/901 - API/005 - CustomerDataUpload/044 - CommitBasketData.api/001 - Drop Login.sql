USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitBasketData.api')
    BEGIN
        DROP LOGIN [CommitBasketData.api]
    END
GO
