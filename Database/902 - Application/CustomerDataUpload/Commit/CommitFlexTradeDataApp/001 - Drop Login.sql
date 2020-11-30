USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexTradeDataApp')
    BEGIN
        DROP LOGIN [CommitFlexTradeDataApp]
    END
GO
