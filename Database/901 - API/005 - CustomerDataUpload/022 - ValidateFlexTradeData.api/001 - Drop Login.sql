USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexTradeData.api')
    BEGIN
        DROP LOGIN [ValidateFlexTradeData.api]
    END
GO
