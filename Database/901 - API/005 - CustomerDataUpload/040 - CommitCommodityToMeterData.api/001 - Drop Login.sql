USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCommodityToMeterData.api')
    BEGIN
        DROP LOGIN [CommitCommodityToMeterData.api]
    END
GO
