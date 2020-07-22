USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProfileClassCodeAttributeId BIGINT = (SELECT ProfileClassAttributeId FROM [Information].[ProfileClassAttribute] WHERE ProfileClassAttributeDescription = 'Profile Class Code')
DECLARE @ProfileClassDescriptionAttributeId BIGINT = (SELECT ProfileClassAttributeId FROM [Information].[ProfileClassAttribute] WHERE ProfileClassAttributeDescription = 'Profile Class Description')

DECLARE @ProfileClassId BIGINT = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'A9A7B9CD-F00B-4C61-B940-85657ACACF22')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '00'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Half-hourly supply (import and export)'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'CAF2D6FF-9C04-4EAA-8FCA-5B4D07FDA498')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '01'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Domestic unrestricted'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '48410A9F-F0E0-40D3-BDB6-B2B9E31CBEEC')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '02'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Domestic Economy meter of two or more rates'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '9D499E35-1AED-4FA3-B8F0-4DD1AF90BDF4')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '03'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic unrestricted'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'FAD5F73B-5057-4FC3-A5EE-94803D195968')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '04'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic Economy 7'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '3FDAD543-AE1D-4765-AA0E-5BA2BE218D6B')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '05'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic, with maximum demand (MD) recording capability and with load factor (LF) less than or equal to 20%'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '87AC1F01-EFA4-40B6-97BE-09A35ADEF781')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '06'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic, with MD recording capability and with LF less than or equal to 30% and greater than 20%'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'BF83C7AC-95DD-4D7E-AB22-5E56026C1754')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '07'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic, with MD recording capability and with LF less than or equal to 40% and greater than 30%'

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'F2C35766-5FC3-4E09-A330-4FA465492493')
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassCodeAttributeId, '08'
EXEC [Information].[ProfileClassDetail_Insert] @CreatedByUserId, @SourceId, @ProfileClassId, @ProfileClassDescriptionAttributeId, 'Non-domestic, with MD recording capability and with LF greater than 40% (also all non-half-hourly export MSIDs)'