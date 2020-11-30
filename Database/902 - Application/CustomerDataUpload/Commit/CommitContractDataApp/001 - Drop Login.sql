USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractDataApp')
    BEGIN
        DROP LOGIN [CommitContractDataApp]
    END
GO
