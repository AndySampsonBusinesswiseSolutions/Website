USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CommodityToTimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CommodityToTimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-20
-- Description:	Insert new mapping of a Commodity to a TimePeriod into [Mapping].[CommodityToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[CommodityToTimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CommodityId BIGINT,
    @TimePeriodId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].CommodityToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        CommodityId,
        TimePeriodId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @CommodityId,
        @TimePeriodId
    )
END
GO
