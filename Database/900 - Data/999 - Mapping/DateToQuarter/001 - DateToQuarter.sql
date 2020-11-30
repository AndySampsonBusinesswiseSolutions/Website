USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO [Mapping].[DateToQuarter]
    (
        CreatedByUserId,
        SourceId,
        DateId,
        QuarterId
    )
SELECT
    @CreatedByUserId,
    @SourceId,
	[Date].[DateId],
	[Quarter].[QuarterId]
FROM 
	[Information].[Date]
INNER JOIN
	[Information].[Quarter]
	ON Quarter.QuarterDescription = (
		CASE
			WHEN DATENAME(mm, DateDescription) = 'January' THEN 'Quarter 1'
			WHEN DATENAME(mm, DateDescription) = 'February' THEN 'Quarter 1'
			WHEN DATENAME(mm, DateDescription) = 'March' THEN 'Quarter 1'
			WHEN DATENAME(mm, DateDescription) = 'April' THEN 'Quarter 2'
			WHEN DATENAME(mm, DateDescription) = 'May' THEN 'Quarter 2'
			WHEN DATENAME(mm, DateDescription) = 'June' THEN 'Quarter 2'
			WHEN DATENAME(mm, DateDescription) = 'July' THEN 'Quarter 3'
			WHEN DATENAME(mm, DateDescription) = 'August' THEN 'Quarter 3'
			WHEN DATENAME(mm, DateDescription) = 'September' THEN 'Quarter 3'
			WHEN DATENAME(mm, DateDescription) = 'October' THEN 'Quarter 4'
			WHEN DATENAME(mm, DateDescription) = 'November' THEN 'Quarter 4'
			WHEN DATENAME(mm, DateDescription) = 'December' THEN 'Quarter 4'
		END
	)
LEFT OUTER JOIN
	[Mapping].[DateToQuarter]
	ON [DateToQuarter].[DateId] = [Date].[DateId]
	AND [DateToQuarter].[QuarterId] = [Quarter].[QuarterId]
WHERE
	[DateToQuarter].[DateToQuarterId] IS NULL