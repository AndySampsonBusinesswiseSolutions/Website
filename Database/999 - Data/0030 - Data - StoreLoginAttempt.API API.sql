USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = '3BBFC2B6-2572-43CD-921A-A237000AC248'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @APIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'API Name', 'StoreLoginAttempt.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTP Application URL', 'http://localhost:5018/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'HTTPS Application URL', 'https://localhost:5019/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'POST Route', 'StoreLoginAttempt/Store'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'QueueGUID'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Required Data Key', 'EmailAddress'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'Prerequisite API GUID', 'CEC56745-C1C5-4E67-805B-159A8A5E991D'
EXEC [Mapping].[APIToProcess_Insert] @UserGUID, @SourceTypeDescription, @APIGUID, 'AF10359F-FD78-4345-9F26-EF5A921E72FD'