USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFixedContractData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempFixedContractData.api]
    END
GO
