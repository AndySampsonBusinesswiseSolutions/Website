USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Customers'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Sites'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Meters'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'SubMeters'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Meter HH Data'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Meter Exemptions'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'SubMeter HH Data'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Fixed Contracts'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Flex Contracts'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Flex Reference Volumes'
EXEC [Customer].[DataUploadValidationErrorSheetAttribute_Insert] @CreatedByUserId, @SourceId, 'Flex Trades'