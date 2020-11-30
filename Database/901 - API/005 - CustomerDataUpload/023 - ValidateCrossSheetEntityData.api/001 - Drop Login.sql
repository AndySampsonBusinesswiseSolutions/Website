USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateCrossSheetEntityData.api')
    BEGIN
        DROP LOGIN [ValidateCrossSheetEntityData.api]
    END
GO