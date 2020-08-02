USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempCustomerData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempCustomerData.api] WITH PASSWORD=N'Mkf2GTm2crKuk6jh', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
