USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexContractDataApp')
    BEGIN
        DROP LOGIN [CommitFlexContractDataApp]
    END
GO
