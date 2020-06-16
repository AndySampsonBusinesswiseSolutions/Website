USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = 'F916F19F-9408-4969-84DC-9905D2FEFB0B'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @APIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'API Name', 'ValidatePageGUID.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTP Application URL', 'http://localhost:5010/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTPS Application URL', 'https://localhost:5011/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'POST Route', 'ValidatePageGUID/Validate'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'QueueGUID'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'PageGUID'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Prerequisite API GUID', '87AFEDA8-6A0F-4143-BF95-E08E78721CF5'
EXEC [Mapping].[APIToProcess_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'AF10359F-FD78-4345-9F26-EF5A921E72FD'