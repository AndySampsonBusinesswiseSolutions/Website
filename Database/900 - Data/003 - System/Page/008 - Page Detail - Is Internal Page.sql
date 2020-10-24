USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM [System].[PageAttribute] WHERE PageAttributeDescription = 'Is Internal Page')

DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '74268A15-79AB-4375-930F-028C99297F38') --Revenue Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '37D34C4F-C9BD-48B1-B3C1-1D95B3BF2211') --Opportunity Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '97E452E6-7C54-49AE-9E3D-6FCC9CC0AB7D') --Supplier Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'F252ED3D-E30E-48A3-B298-5709D3A154B2') --Supplier Product Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'B974CFB4-AE27-4A72-A1D7-927B84C5CB62') --Manage Users
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '80B1CC99-7C91-4D07-A541-9D69AC4CC304') --Manage Customers
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, True