USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = '87AFEDA8-6A0F-4143-BF95-E08E78721CF5'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @APIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'API Name', 'ValidateProcessGUID.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTP Application URL', 'http://localhost:5006/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTPS Application URL', 'https://localhost:5007/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'POST Route', 'ValidateProcessGUID/Validate'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'QueueGUID'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'ProcessGUID'