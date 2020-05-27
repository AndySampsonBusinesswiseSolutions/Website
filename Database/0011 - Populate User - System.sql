USE [EMaaS]
GO

DECLARE @GUID UNIQUEIDENTIFIER = '743E21EE-2185-45D4-9003-E35060B751E2'

IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[User] WHERE GUID = @GUID)
    BEGIN
        INSERT INTO [Administration.User].[User]
        (
            GUID
        )
        VALUES
        (
            @GUID
        )
    END