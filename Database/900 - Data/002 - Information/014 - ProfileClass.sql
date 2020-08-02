USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, 'A9A7B9CD-F00B-4C61-B940-85657ACACF22' --00
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, 'CAF2D6FF-9C04-4EAA-8FCA-5B4D07FDA498' --01
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, '48410A9F-F0E0-40D3-BDB6-B2B9E31CBEEC' --02
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, '9D499E35-1AED-4FA3-B8F0-4DD1AF90BDF4' --03
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, 'FAD5F73B-5057-4FC3-A5EE-94803D195968' --04
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, '3FDAD543-AE1D-4765-AA0E-5BA2BE218D6B' --05
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, '87AC1F01-EFA4-40B6-97BE-09A35ADEF781' --06
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, 'BF83C7AC-95DD-4D7E-AB22-5E56026C1754' --07
EXEC [Information].[ProfileClass_Insert] @CreatedByUserId, @SourceId, 'F2C35766-5FC3-4E09-A330-4FA465492493' --08