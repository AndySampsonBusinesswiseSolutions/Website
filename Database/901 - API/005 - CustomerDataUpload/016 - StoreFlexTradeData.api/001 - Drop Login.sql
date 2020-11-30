USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexTradeData.api')
    BEGIN
        DROP LOGIN [StoreFlexTradeData.api]
    END
GO
