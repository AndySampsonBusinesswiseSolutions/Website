USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddressPasswordMappingApp')
    BEGIN
        CREATE LOGIN [ValidateEmailAddressPasswordMappingApp] WITH PASSWORD=N'GQzD2!aZNvffr*zC', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO