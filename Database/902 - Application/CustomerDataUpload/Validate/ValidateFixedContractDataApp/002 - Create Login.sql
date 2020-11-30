USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFixedContractDataApp')
    BEGIN
       CREATE LOGIN [ValidateFixedContractDataApp] WITH PASSWORD=N'aHG2nFuzttAHQDCN', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
