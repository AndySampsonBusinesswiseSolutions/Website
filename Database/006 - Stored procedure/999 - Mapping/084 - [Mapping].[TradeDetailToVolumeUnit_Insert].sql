USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[TradeDetailToVolumeUnit_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[TradeDetailToVolumeUnit_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new mapping of a TradeDetail to a VolumeUnit into [Mapping].[TradeDetailToVolumeUnit] table
-- =============================================

ALTER PROCEDURE [Mapping].[TradeDetailToVolumeUnit_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeDetailId BIGINT,
    @VolumeUnitId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].TradeDetailToVolumeUnit
    (
        CreatedByUserId,
        SourceId,
        TradeDetailId,
        VolumeUnitId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @TradeDetailId,
        @VolumeUnitId
    )
END
GO
