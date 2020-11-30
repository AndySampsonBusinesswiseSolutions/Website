USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterExemptionToMeterExemptionProduct_GetByMeterExemptionIdAndMeterExemptionProductId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterExemptionToMeterExemptionProduct_GetByMeterExemptionIdAndMeterExemptionProductId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get MeterExemptionToMeterExemptionProduct info from [Mapping].[MeterExemptionToMeterExemptionProduct] table by MeterExemption Id and MeterExemptionProduct Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterExemptionToMeterExemptionProduct_GetByMeterExemptionIdAndMeterExemptionProductId]
    @MeterExemptionId BIGINT,
    @MeterExemptionProductId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        MeterExemptionToMeterExemptionProductId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterExemptionId,
        MeterExemptionProductId
    FROM 
        [Mapping].[MeterExemptionToMeterExemptionProduct]
    WHERE 
        MeterExemptionId = @MeterExemptionId
        AND MeterExemptionProductId = @MeterExemptionProductId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
