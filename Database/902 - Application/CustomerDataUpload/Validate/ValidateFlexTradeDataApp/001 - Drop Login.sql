USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexTradeDataApp')
    BEGIN
        DROP LOGIN [ValidateFlexTradeDataApp]
    END
GO
