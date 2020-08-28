USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CleanUpCustomerDataUploadTempData.api')
    BEGIN
        DROP LOGIN [CleanUpCustomerDataUploadTempData.api]
    END
GO
