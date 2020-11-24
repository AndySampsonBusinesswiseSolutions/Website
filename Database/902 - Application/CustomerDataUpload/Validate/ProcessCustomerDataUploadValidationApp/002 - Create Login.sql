USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ProcessCustomerDataUploadValidationApp')
    BEGIN
       CREATE LOGIN [ProcessCustomerDataUploadValidationApp] WITH PASSWORD=N'VcTpcaaHYSFVa5bB', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
