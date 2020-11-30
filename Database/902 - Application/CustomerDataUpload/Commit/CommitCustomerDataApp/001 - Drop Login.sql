USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerDataApp')
    BEGIN
        DROP LOGIN [CommitCustomerDataApp]
    END
GO
