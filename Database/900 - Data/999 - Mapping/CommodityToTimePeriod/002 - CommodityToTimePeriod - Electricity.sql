USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @CommodityId BIGINT = (SELECT CommodityId FROM [Information].[Commodity] WHERE CommodityDescription = 'Electricity')
DECLARE @DayTimePeriodId BIGINT = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '00:00:00.0000000' AND EndTime = '00:00:00.0000000')

DECLARE @TimePeriodId BIGINT

DECLARE TimePeriodCursor CURSOR FOR
SELECT TimePeriodId
FROM [Information].[TimePeriod]
WHERE TimePeriodId NOT IN (@DayTimePeriodId)

OPEN TimePeriodCursor

FETCH NEXT FROM TimePeriodCursor
INTO @TimePeriodId

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Mapping].[CommodityToTimePeriod_Insert] 
			@CreatedByUserId, 
			@SourceId, 
			@CommodityId, 
			@TimePeriodId

		FETCH NEXT FROM TimePeriodCursor
		INTO @TimePeriodId
	END
CLOSE TimePeriodCursor;
DEALLOCATE TimePeriodCursor;