USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCommodityToMeterDataApp')
    BEGIN
       CREATE LOGIN [CommitCommodityToMeterDataApp] WITH PASSWORD=N'dJPDrGp8DPndzw9w', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
