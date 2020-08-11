USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitTradeToBasketData.api')
    BEGIN
        DROP LOGIN [CommitTradeToBasketData.api]
    END
GO
