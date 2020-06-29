USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @GroupDescription TABLE
(
	ForecastGroupDescription VARCHAR(100)
)

INSERT INTO @GroupDescription
	(
		ForecastGroupDescription
	)
SELECT
	OrdinalNumberDescription + ' ' + DayOfTheWeekDescription + ' in ' + MonthDescription
FROM
	[Information].OrdinalNumber
CROSS APPLY
	[Information].DayOfTheWeek
CROSS APPLY
	[Information].Month

DECLARE @ForecastGroupDescription VARCHAR(255)

DECLARE ForecastGroupDescriptionCursor CURSOR FOR
SELECT ForecastGroupDescription
FROM @GroupDescription

OPEN ForecastGroupDescriptionCursor

FETCH NEXT FROM ForecastGroupDescriptionCursor
INTO @ForecastGroupDescription

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [DemandForecast].[ForecastGroup_Insert] @CreatedByUserId, @SourceId, @ForecastGroupDescription

		FETCH NEXT FROM ForecastGroupDescriptionCursor
		INTO @ForecastGroupDescription
	END
CLOSE ForecastGroupDescriptionCursor;
DEALLOCATE ForecastGroupDescriptionCursor;