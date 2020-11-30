USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexTradeDataApp')
    BEGIN
        DROP LOGIN [StoreFlexTradeDataApp]
    END
GO
