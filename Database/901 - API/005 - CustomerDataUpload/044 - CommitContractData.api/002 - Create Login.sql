USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractData.api')
    BEGIN
       CREATE LOGIN [CommitContractData.api] WITH PASSWORD=N'wsxbn8B2jTb9bDFM', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
