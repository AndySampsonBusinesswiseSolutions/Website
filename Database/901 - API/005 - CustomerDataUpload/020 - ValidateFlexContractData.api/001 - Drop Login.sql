USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexContractData.api')
    BEGIN
        DROP LOGIN [ValidateFlexContractData.api]
    END
GO
