USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[DateToWeek_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[DateToWeek_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new mapping of a Date to a Week into [Mapping].[DateToWeek] table
-- =============================================

ALTER PROCEDURE [Mapping].[DateToWeek_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateId BIGINT,
    @WeekId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].DateToWeek
    (
        CreatedByUserId,
        SourceId,
        DateId,
        WeekId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateId,
        @WeekId
    )
END
GO
