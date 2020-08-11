USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexTradeData.api')
    BEGIN
        DROP LOGIN [CommitFlexTradeData.api]
    END
GO
