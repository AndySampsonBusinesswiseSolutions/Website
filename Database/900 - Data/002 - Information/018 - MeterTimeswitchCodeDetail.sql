USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @MeterTimeswitchCodeRangeStartAttributeId BIGINT = (SELECT MeterTimeswitchCodeAttributeId FROM [Information].[MeterTimeswitchCodeAttribute] WHERE MeterTimeswitchCodeAttributeDescription = 'Meter Timeswitch Code Range Start')
DECLARE @MeterTimeswitchCodeRangeEndAttributeId BIGINT = (SELECT MeterTimeswitchCodeAttributeId FROM [Information].[MeterTimeswitchCodeAttribute] WHERE MeterTimeswitchCodeAttributeDescription = 'Meter Timeswitch Code Range End')
DECLARE @MeterTimeswitchCodeRangeDescriptionAttributeId BIGINT = (SELECT MeterTimeswitchCodeAttributeId FROM [Information].[MeterTimeswitchCodeAttribute] WHERE MeterTimeswitchCodeAttributeDescription = 'Meter Timeswitch Code Range Description')

DECLARE @MeterTimeswitchCodeId BIGINT = (SELECT MeterTimeswitchCodeId FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = '3811CB5D-8B34-4D89-8CC4-7D046E730DCB')
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeStartAttributeId, '001'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeEndAttributeId, '399'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeDescriptionAttributeId, 'DNO specific'

SET @MeterTimeswitchCodeId = (SELECT MeterTimeswitchCodeId FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = '51B46BF5-9943-49B8-B3B5-3786DBDF43C3')
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeStartAttributeId, '400'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeEndAttributeId, '499'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeDescriptionAttributeId, 'Reserved'

SET @MeterTimeswitchCodeId = (SELECT MeterTimeswitchCodeId FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = '4F6CB615-1636-4C2C-AA7B-60D3F2E37536')
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeStartAttributeId, '500'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeEndAttributeId, '509'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeDescriptionAttributeId, 'Codes for related Metering Systems – common across the Industry'

SET @MeterTimeswitchCodeId = (SELECT MeterTimeswitchCodeId FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = '392FAEAF-4157-4EEA-A71E-CE9CCCB3D658')
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeStartAttributeId, '510'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeEndAttributeId, '799'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeDescriptionAttributeId, 'Codes for related Metering Systems – DNO specific'

SET @MeterTimeswitchCodeId = (SELECT MeterTimeswitchCodeId FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = '4FEE087D-0074-4FA4-9FD0-C60A76FA2491')
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeStartAttributeId, '800'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeEndAttributeId, '999'
EXEC [Information].[MeterTimeswitchCodeDetail_Insert] @CreatedByUserId, @SourceId, @MeterTimeswitchCodeId, @MeterTimeswitchCodeRangeDescriptionAttributeId, 'Codes common across the Industry'