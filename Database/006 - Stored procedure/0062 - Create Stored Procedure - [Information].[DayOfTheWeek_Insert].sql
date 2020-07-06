USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[DayOfTheWeek_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[DayOfTheWeek_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new DayOfTheWeek into [Information].[DayOfTheWeek] table
-- =============================================

ALTER PROCEDURE [Information].[DayOfTheWeek_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DayOfTheWeekDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[DayOfTheWeek] WHERE DayOfTheWeekDescription = @DayOfTheWeekDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[DayOfTheWeek]
            (
                CreatedByUserId,
                SourceId,
                DayOfTheWeekDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @DayOfTheWeekDescription
            )
        END
END
GO
