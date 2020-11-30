USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get MeterExemptionDetail info from [Customer].[MeterExemptionDetail] table by MeterExemption Id and MeterExemption Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId]
    @MeterExemptionId BIGINT,
    @MeterExemptionAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        MeterExemptionDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterExemptionId,
        MeterExemptionAttributeId,
        MeterExemptionDetailDescription
    FROM 
        [Customer].[MeterExemptionDetail] 
    WHERE 
        MeterExemptionId = @MeterExemptionId
        AND MeterExemptionAttributeId = @MeterExemptionAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
