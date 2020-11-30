USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateMonthForecast.api')
    BEGIN
       CREATE LOGIN [CreateMonthForecast.api] WITH PASSWORD=N'9JQtHgkA2CcnaE67', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
