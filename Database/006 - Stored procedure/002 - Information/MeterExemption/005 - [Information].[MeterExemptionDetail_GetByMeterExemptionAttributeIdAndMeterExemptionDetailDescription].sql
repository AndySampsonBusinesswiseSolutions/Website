USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-23
-- Description:	Get MeterExemption Detail info from [Information].[MeterExemptionDetail] table by MeterExemption Attribute Id And MeterExemption Detail Description
-- =============================================

ALTER PROCEDURE [Information].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription]
    @MeterExemptionAttributeId BIGINT,
    @MeterExemptionDetailDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-23 -> Andrew Sampson -> Initial development of script
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
        [Information].[MeterExemptionDetail] 
    WHERE 
        MeterExemptionAttributeId = @MeterExemptionAttributeId
        AND MeterExemptionDetailDescription = @MeterExemptionDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
