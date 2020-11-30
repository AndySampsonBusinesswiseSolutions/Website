USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetFlexSpecificProfile.api')
    BEGIN
       CREATE LOGIN [GetFlexSpecificProfile.api] WITH PASSWORD=N'YeGMx45xZ9Uxy2sB', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
