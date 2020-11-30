USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[RateTypeToRateUnit_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[RateTypeToRateUnit_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new mapping of a RateType to a RateUnit into [Mapping].[RateTypeToRateUnit] table
-- =============================================

ALTER PROCEDURE [Mapping].[RateTypeToRateUnit_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @RateTypeId BIGINT,
    @RateUnitId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].RateTypeToRateUnit
    (
        CreatedByUserId,
        SourceId,
        RateTypeId,
        RateUnitId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @RateTypeId,
        @RateUnitId
    )
END
GO
