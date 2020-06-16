USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = 'CEC56745-C1C5-4E67-805B-159A8A5E991D'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @APIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'API Name', 'ValidateEmailAddressPasswordMapping.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTP Application URL', 'http://localhost:5016/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTPS Application URL', 'https://localhost:5017/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'POST Route', 'ValidateEmailAddressPasswordMapping/Validate'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'QueueGUID'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'Password'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'EmailAddress'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Prerequisite API GUID', '26FEFFE8-49F7-4458-98ED-FD5F6C65C7C2'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Prerequisite API GUID', '99681B37-575F-47E5-95E3-608063EA513E'
EXEC [Mapping].[APIToProcess_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'AF10359F-FD78-4345-9F26-EF5A921E72FD'