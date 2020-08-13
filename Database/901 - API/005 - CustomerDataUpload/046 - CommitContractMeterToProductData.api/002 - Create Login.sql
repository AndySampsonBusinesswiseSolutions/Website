USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractMeterToProductData.api')
    BEGIN
       CREATE LOGIN [CommitContractMeterToProductData.api] WITH PASSWORD=N'', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
