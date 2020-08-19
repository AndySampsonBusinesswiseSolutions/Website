USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitGridSupplyPointToMeterData.api')
    BEGIN
       CREATE LOGIN [CommitGridSupplyPointToMeterData.api] WITH PASSWORD=N'TLbyfyMNz2NTGMzb', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
