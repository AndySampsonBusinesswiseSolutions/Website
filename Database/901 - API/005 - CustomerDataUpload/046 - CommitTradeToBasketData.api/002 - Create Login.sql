USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitTradeToBasketData.api')
    BEGIN
       CREATE LOGIN [CommitTradeToBasketData.api] WITH PASSWORD=N'uPXLHnw3FFbVMCrw', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
