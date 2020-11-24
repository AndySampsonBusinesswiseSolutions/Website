USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexContractDataApp')
    BEGIN
        DROP LOGIN [ValidateFlexContractDataApp]
    END
GO
