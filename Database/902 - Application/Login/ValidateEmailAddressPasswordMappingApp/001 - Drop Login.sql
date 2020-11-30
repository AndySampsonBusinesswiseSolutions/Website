USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddressPasswordMappingApp')
    BEGIN
        DROP LOGIN [ValidateEmailAddressPasswordMappingApp]
    END
GO