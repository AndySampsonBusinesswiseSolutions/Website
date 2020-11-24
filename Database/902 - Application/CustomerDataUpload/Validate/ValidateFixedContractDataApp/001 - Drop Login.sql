USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFixedContractDataApp')
    BEGIN
        DROP LOGIN [ValidateFixedContractDataApp]
    END
GO
