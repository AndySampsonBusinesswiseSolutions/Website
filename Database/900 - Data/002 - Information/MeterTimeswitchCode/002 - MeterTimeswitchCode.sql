USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[MeterTimeswitchCode_Insert] @CreatedByUserId, @SourceId, '3811CB5D-8B34-4D89-8CC4-7D046E730DCB' --001-399
EXEC [Information].[MeterTimeswitchCode_Insert] @CreatedByUserId, @SourceId, '51B46BF5-9943-49B8-B3B5-3786DBDF43C3' --400-499
EXEC [Information].[MeterTimeswitchCode_Insert] @CreatedByUserId, @SourceId, '4F6CB615-1636-4C2C-AA7B-60D3F2E37536' --500-509
EXEC [Information].[MeterTimeswitchCode_Insert] @CreatedByUserId, @SourceId, '392FAEAF-4157-4EEA-A71E-CE9CCCB3D658' --510-799
EXEC [Information].[MeterTimeswitchCode_Insert] @CreatedByUserId, @SourceId, '4FEE087D-0074-4FA4-9FD0-C60A76FA2491' --800-999