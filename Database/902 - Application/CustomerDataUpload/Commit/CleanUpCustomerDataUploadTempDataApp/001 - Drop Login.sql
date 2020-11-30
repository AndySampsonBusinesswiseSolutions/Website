USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CleanUpCustomerDataUploadTempDataApp')
    BEGIN
        DROP LOGIN [CleanUpCustomerDataUploadTempDataApp]
    END
GO
