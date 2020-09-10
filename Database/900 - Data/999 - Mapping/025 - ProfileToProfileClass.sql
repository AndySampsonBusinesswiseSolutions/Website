USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ProfileClassId BIGINT = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'CAF2D6FF-9C04-4EAA-8FCA-5B4D07FDA498')
DECLARE @ProfileId BIGINT = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '860DB634-8C06-46EC-8335-5136F7BC5D07')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '48410A9F-F0E0-40D3-BDB6-B2B9E31CBEEC')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '0EEBDC0D-C4BA-4D02-B358-BA9519BCEC67')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '9D499E35-1AED-4FA3-B8F0-4DD1AF90BDF4')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = 'C06D87A0-D7CC-40CD-B512-0262007EA179')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'FAD5F73B-5057-4FC3-A5EE-94803D195968')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = 'E1BFF30D-6C08-4BD5-BB07-336953BD7511')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '3FDAD543-AE1D-4765-AA0E-5BA2BE218D6B')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '3342CC78-582E-499B-B053-093F50365FAD')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = '87AC1F01-EFA4-40B6-97BE-09A35ADEF781')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '3D45D041-22B8-4A53-B5E1-B6C7F932285C')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'BF83C7AC-95DD-4D7E-AB22-5E56026C1754')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '01BD095A-4F09-4B98-8978-21BFB70C179E')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId

SET @ProfileClassId = (SELECT ProfileClassId FROM [Information].[ProfileClass] WHERE ProfileClassGUID = 'F2C35766-5FC3-4E09-A330-4FA465492493')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '2E3E5721-2849-47CE-ABC9-27C7D2A42310')
EXEC [Mapping].[ProfileToProfileClass_Insert] @CreatedByUserId, @SourceId, @ProfileId, @ProfileClassId