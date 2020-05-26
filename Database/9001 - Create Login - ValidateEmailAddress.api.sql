USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddress.api')
    BEGIN
        CREATE LOGIN [ValidateEmailAddress.api] WITH PASSWORD=N'~/@X@4Xc88$\~h;]', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddress.api')
    BEGIN
        CREATE USER [ValidateEmailAddress.api] FOR LOGIN [ValidateEmailAddress.api]
    END
GO

USE [EMaaS]
GO

ALTER USER [ValidateEmailAddress.api] WITH DEFAULT_SCHEMA=[System]
GO

USE [EMaaS]
GO

ALTER ROLE [db_datareader] ADD MEMBER [ValidateEmailAddress.api]
GO