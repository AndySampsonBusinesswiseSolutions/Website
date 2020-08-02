USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'DetermineFileType.api')
    BEGIN
       CREATE LOGIN [DetermineFileType.api] WITH PASSWORD=N'dp2juZYYbdjkh43c', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
