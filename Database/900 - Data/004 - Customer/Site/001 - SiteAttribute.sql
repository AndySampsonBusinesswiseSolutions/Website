USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site Name'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site Address'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site Town'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site County'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site PostCode'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Site Description'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Name'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Telephone Number'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Email Address'
EXEC [Customer].[SiteAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Role'