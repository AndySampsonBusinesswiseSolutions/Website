USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE 
	@DateId BIGINT,
	@DateDescription VARCHAR(255),
	@WeekId BIGINT

DECLARE DateDescriptionCursor CURSOR FOR
SELECT DateId, DateDescription
FROM [Information].[Date]

OPEN DateDescriptionCursor

FETCH NEXT FROM DateDescriptionCursor
INTO @DateId, @DateDescription

WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @WeekId = (SELECT WeekId FROM [Information].[Week] WHERE [Week].WeekDescription = DATEPART(wk, @DateDescription))
		EXEC [Mapping].[DateToWeek_Insert] @CreatedByUserId, @SourceId, @DateDescription, @WeekId

		FETCH NEXT FROM DateDescriptionCursor
		INTO @DateDescription
	END
CLOSE DateDescriptionCursor;
DEALLOCATE DateDescriptionCursor;