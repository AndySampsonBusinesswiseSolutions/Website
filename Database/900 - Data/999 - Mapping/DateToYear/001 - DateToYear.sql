USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO
	[Mapping].[DateToYear]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		YearId
	)
SELECT
	@CreatedByUserId,
	@SourceId,
	[Date].[DateId],
	[Year].[YearId]
FROM 
	[Information].[Date]
INNER JOIN
	[Information].[Year]
	ON [Year].YearDescription = DATEPART(year, [Date].[DateDescription])
LEFT OUTER JOIN
	[Mapping].[DateToYear]
	ON [DateToYear].[DateId] = [Date].[DateId]
	AND [DateToYear].[YearId] = [Year].[YearId]
WHERE
	[DateToYear].[DateToYearId] IS NULL