USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFixedContractDataApp')
    BEGIN
        DROP LOGIN [CommitFixedContractDataApp]
    END
GO
