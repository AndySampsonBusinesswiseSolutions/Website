USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MonthToQuarter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MonthToQuarter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new mapping of a Month to a Quarter into [Mapping].[MonthToQuarter] table
-- =============================================

ALTER PROCEDURE [Mapping].[MonthToQuarter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MonthId BIGINT,
    @QuarterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].MonthToQuarter
    (
        CreatedByUserId,
        SourceId,
        MonthId,
        QuarterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MonthId,
        @QuarterId
    )
END
GO
