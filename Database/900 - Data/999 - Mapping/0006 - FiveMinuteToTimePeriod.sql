USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @StartTime TIME = '00:00:00'
DECLARE @EndTime TIME = '23:55:00'
DECLARE @TempTime TIME
DECLARE @TempEndTime TIME
DECLARE @TimePeriodId BIGINT
DECLARE @FiveMinuteId BIGINT
DECLARE @Counter INT

SET @TempTime = @StartTime
SET @Counter = 1

WHILE @TempTime <= @EndTime
	BEGIN
		SET @TempEndTime = DATEADD(MINUTE, 5, @TempTime)

        SET @TimePeriodId = (
            SELECT
                TimePeriodId
            FROM
                [Information].[TimePeriod]
            WHERE
                StartTime = FORMAT(DATEPART(HOUR, @TempTime), '00') + ':' + FORMAT(DATEPART(MINUTE, @TempTime), '00') + ':' + FORMAT(DATEPART(SECOND, @TempTime), '00')
                AND EndTime = FORMAT(DATEPART(HOUR, @TempEndTime), '00') + ':' + FORMAT(DATEPART(MINUTE, @TempEndTime), '00') + ':' + FORMAT(DATEPART(SECOND, @TempEndTime), '00')
                AND EffectiveToDateTime = '9999-12-31'
        )

        SET @FiveMinuteId = (
            SELECT
                FiveMinuteId
            FROM
                [Information].[FiveMinute]
            WHERE
                FiveMinuteDescription = 'Five Minute ' + @Counter
        )

        EXEC [Mapping].[FiveMinuteToTimePeriod] @CreatedByUserId, @SourceId, @FiveMinuteId, @TimePeriodId

		SET @TempTime = DATEADD(MINUTE, 5, @TempTime)
        SET @Counter = @Counter + 1
	END