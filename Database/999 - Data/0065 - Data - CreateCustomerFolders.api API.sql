USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = '2AE6E5B2-1D5F-431B-9BCE-A5CDDB52B493'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[API_Insert] @CreatedByUserId, @SourceId, @APIGUID

DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = @APIGUID)

DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'API Name')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'CreateCustomerFolders.api'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'HTTP Application URL')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'http://localhost:5030/'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'HTTPS Application URL')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'https://localhost:5031/'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'POST Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'CreateCustomerFolders/Create'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Required Data Key')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ProcessGUID'
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'CustomerGUID'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Prerequisite API GUID')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '1B2E2BA3-D538-47E0-9044-BBBFC6BF3892'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Timeout Seconds')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '600'

SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'CreateCustomerFolders/IsRunning'

DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID ='D39E768A-D06D-4EB3-80E3-895EDC556A6B')
EXEC [Mapping].[APIToProcess_Insert] @CreatedByUserId, @SourceId, @APIId, @ProcessId
