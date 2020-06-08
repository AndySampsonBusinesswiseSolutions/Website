USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = '38D3A9E1-A060-4464-B971-8DC523B6A42D'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @APIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'API Name', 'ArchiveProcessQueue.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTP Application URL', 'http://localhost:5008/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTPS Application URL', 'https://localhost:5009/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'POST Route', 'ArchiveProcessQueue/Archive'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'QueueGUID'