USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateCrossSheetEntityDataApp')
    BEGIN
       CREATE LOGIN [ValidateCrossSheetEntityDataApp] WITH PASSWORD=N'SQTP72kBj36cntMn', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO