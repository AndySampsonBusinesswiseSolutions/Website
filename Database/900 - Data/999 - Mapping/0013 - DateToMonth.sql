USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE 
	@DateId BIGINT,
	@DateDescription VARCHAR(255),
	@MonthId BIGINT

DECLARE DateDescriptionCursor CURSOR FOR
SELECT DateId, DateDescription
FROM [Information].[Date]

OPEN DateDescriptionCursor

FETCH NEXT FROM DateDescriptionCursor
INTO @DateId, @DateDescription

WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE [Month].MonthDescription = DATEPART(mm, @DateDescription))
		EXEC [Mapping].[DateToMonth_Insert] @CreatedByUserId, @SourceId, @DateDescription, @MonthId

		FETCH NEXT FROM DateDescriptionCursor
		INTO @DateDescription
	END
CLOSE DateDescriptionCursor;
DEALLOCATE DateDescriptionCursor;