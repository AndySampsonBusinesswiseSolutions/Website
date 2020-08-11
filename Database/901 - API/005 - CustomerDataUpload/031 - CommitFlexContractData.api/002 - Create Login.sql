USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexContractData.api')
    BEGIN
       CREATE LOGIN [CommitFlexContractData.api] WITH PASSWORD=N'MnXB6w8fSZuKuHL9', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
