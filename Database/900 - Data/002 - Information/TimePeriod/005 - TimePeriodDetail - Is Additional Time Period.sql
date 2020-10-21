USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @IsAdditionalTimePeriodId BIGINT = (SELECT TimePeriodAttributeId FROM [Information].[TimePeriodAttribute] WHERE TimePeriodAttributeDescription = 'Is Additional Time Period')

DECLARE @TimePeriodId BIGINT = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:31:00.0000000' AND EndTime = '01:36:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:36:00.0000000' AND EndTime = '01:41:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:41:00.0000000' AND EndTime = '01:46:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:46:00.0000000' AND EndTime = '01:51:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:51:00.0000000' AND EndTime = '01:56:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:56:00.0000000' AND EndTime = '02:01:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:01:00.0000000' AND EndTime = '02:06:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:06:00.0000000' AND EndTime = '02:11:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:11:00.0000000' AND EndTime = '02:16:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:16:00.0000000' AND EndTime = '02:21:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:21:00.0000000' AND EndTime = '02:26:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '02:26:00.0000000' AND EndTime = '02:31:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:01:00.0000000' AND EndTime = '01:31:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'

SET @TimePeriodId = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '01:02:00.0000000' AND EndTime = '01:32:00.0000000')
EXEC [Information].[TimePeriodDetail_Insert] @CreatedByUserId, @SourceId, @TimePeriodId, @IsAdditionalTimePeriodId, 'True'