USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractToMeterData.api')
    BEGIN
       CREATE LOGIN [CommitContractToMeterData.api] WITH PASSWORD=N'6TKhAGYAgf85Xzm4', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
