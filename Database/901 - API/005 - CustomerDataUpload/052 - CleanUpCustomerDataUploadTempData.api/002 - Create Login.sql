USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CleanUpCustomerDataUploadTempData.api')
    BEGIN
       CREATE LOGIN [CleanUpCustomerDataUploadTempData.api] WITH PASSWORD=N'NEvn8kdQ3JThhjkd', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
