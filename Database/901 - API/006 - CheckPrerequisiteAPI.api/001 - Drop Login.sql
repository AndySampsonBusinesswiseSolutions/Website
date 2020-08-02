USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CheckPrerequisiteAPI.api')
    BEGIN
        DROP LOGIN [CheckPrerequisiteAPI.api]
    END
GO