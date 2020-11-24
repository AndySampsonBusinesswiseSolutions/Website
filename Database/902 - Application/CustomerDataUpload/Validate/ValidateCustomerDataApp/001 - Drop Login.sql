USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateCustomerDataApp')
    BEGIN
        DROP LOGIN [ValidateCustomerDataApp]
    END
GO
