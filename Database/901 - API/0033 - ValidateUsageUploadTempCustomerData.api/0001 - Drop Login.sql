USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempCustomerData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempCustomerData.api]
    END
GO
