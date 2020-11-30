USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFixedContractDataApp')
    BEGIN
       CREATE LOGIN [CommitFixedContractDataApp] WITH PASSWORD=N'PQrhQL3PCrchDXnj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
