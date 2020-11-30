USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetMeterSpecificProfile.api')
    BEGIN
       CREATE LOGIN [GetMeterSpecificProfile.api] WITH PASSWORD=N'qNSgD6G3uPZYJMAh', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
