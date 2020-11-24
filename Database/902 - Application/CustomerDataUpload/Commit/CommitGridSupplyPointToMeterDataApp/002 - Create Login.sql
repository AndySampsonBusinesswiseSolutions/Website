USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitGridSupplyPointToMeterDataApp')
    BEGIN
       CREATE LOGIN [CommitGridSupplyPointToMeterDataApp] WITH PASSWORD=N'TLbyfyMNz2NTGMzb', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
