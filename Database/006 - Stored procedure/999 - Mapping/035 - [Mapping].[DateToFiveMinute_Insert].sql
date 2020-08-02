USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[DateToFiveMinute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[DateToFiveMinute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new mapping of a Date to a FiveMinute into [Mapping].[DateToFiveMinute] table
-- =============================================

ALTER PROCEDURE [Mapping].[DateToFiveMinute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateId BIGINT,
    @FiveMinuteId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].DateToFiveMinute
    (
        CreatedByUserId,
        SourceId,
        DateId,
        FiveMinuteId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateId,
        @FiveMinuteId
    )
END
GO
