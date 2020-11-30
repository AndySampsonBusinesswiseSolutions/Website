USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToMeterExemptionToMeterExemptionProduct_GetByMeterToMeterExemptionIdAndMeterExemptionProductId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToMeterExemptionToMeterExemptionProduct_GetByMeterToMeterExemptionIdAndMeterExemptionProductId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get MeterToMeterExemptionToMeterExemptionProduct info from [Mapping].[MeterToMeterExemptionToMeterExemptionProduct] table by MeterExemption Id and MeterExemptionProduct Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToMeterExemptionToMeterExemptionProduct_GetByMeterToMeterExemptionIdAndMeterExemptionProductId]
    @MeterToMeterExemptionId BIGINT,
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
        MeterToMeterExemptionToMeterExemptionProductId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterToMeterExemptionId,
        MeterExemptionProductId
    FROM 
        [Mapping].[MeterToMeterExemptionToMeterExemptionProduct]
    WHERE 
        MeterToMeterExemptionId = @MeterToMeterExemptionId
        AND MeterExemptionProductId = @MeterExemptionProductId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
