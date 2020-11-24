USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCommodityToMeterDataApp')
    BEGIN
        DROP LOGIN [CommitCommodityToMeterDataApp]
    END
GO
