USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Customer Name'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Address Lines'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Address Town'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Address County'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Address PostCode'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Name'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Telephone Number'
EXEC [Customer].[CustomerAttribute_Insert] @CreatedByUserId, @SourceId, 'Contact Email Address'