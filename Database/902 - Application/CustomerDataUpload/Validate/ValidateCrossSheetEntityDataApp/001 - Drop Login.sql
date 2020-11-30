USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateCrossSheetEntityDataApp')
    BEGIN
        DROP LOGIN [ValidateCrossSheetEntityDataApp]
    END
GO