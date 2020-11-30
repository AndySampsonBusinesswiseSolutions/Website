USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Date_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Date_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new Date into [Information].[Date] table
-- =============================================

ALTER PROCEDURE [Information].[Date_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- 2020-07-29 -> Andrew Sampson -> Added WeekId
    -- 2020-08-01 -> Andrew Sampson -> Removed DayOfTheWeek, Week, Month and Year
    --                                 as these are now handled in mapping stored procedures                                       
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[Date]
    (
        CreatedByUserId,
        SourceId,
        DateDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateDescription
    )
END
GO