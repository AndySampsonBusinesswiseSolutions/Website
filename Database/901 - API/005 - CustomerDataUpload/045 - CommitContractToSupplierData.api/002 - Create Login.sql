USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractToSupplierData.api')
    BEGIN
       CREATE LOGIN [CommitContractToSupplierData.api] WITH PASSWORD=N'YVfsgbnrzGh3SCcE', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
