USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetFlexSpecificProfile.api')
    BEGIN
        DROP LOGIN [GetFlexSpecificProfile.api]
    END
GO
