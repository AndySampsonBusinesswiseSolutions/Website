USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @GroupDescription TABLE
(
	ForecastGroupDescription VARCHAR(255)
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

INSERT INTO @GroupDescription
	(
		ForecastGroupDescription
	)
SELECT
	DayOfTheWeekDescription + SpecialDayDescription
FROM
	[Information].DayOfTheWeek
CROSS APPLY
	(
		SELECT ' New Year''s Day' AS SpecialDayDescription
		UNION SELECT ' Christmas Day' AS SpecialDayDescription
		UNION SELECT ' Boxing Day' AS SpecialDayDescription
		UNION SELECT ' Christmas Eve' AS SpecialDayDescription
	) SpecialDay

INSERT INTO @GroupDescription
	(
		ForecastGroupDescription
	)
SELECT
	'Last ' + DayOfTheWeekDescription + ' in ' + MonthDescription
FROM
	[Information].DayOfTheWeek
CROSS APPLY
	[Information].Month

INSERT INTO @GroupDescription
	(
		ForecastGroupDescription
	)
SELECT
	'Penultimate ' + DayOfTheWeekDescription + ' in ' + MonthDescription
FROM
	[Information].DayOfTheWeek
CROSS APPLY
	[Information].Month

INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekday Christmas Eve')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekend Christmas Eve')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Christmas Eve')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekday Christmas Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekend Christmas Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Christmas Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Monday Christmas Day Substitute')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Tuesday Christmas Day Substitute')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekday Boxing Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekend Boxing Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Boxing Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Monday Boxing Day Substitute')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Tuesday Boxing Day Substitute')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekday New Year''s Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Weekend New Year''s Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('New Year''s Day')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Good Friday in March')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Good Friday in April')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Good Friday in May')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Good Friday')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Sunday in March')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Sunday in April')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Sunday in May')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Sunday')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Monday in March')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Monday in April')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Monday in May')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Easter Monday')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Saturday on Easter Weekend in March')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Saturday on Easter Weekend in April')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Saturday on Easter Weekend in May')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Saturday on Easter Weekend')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Early May Bank Holiday')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Spring Bank Holiday')
INSERT INTO @GroupDescription (ForecastGroupDescription) VALUES ('Summer Bank Holiday')

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