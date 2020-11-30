USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitBasketDataApp')
    BEGIN
        DROP LOGIN [CommitBasketDataApp]
    END
GO
