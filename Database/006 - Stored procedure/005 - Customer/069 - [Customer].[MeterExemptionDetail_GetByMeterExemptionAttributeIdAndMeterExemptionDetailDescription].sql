USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get MeterExemptionDetail info from [Customer].[MeterExemptionDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription]
    @MeterExemptionAttributeId BIGINT,
    @MeterExemptionDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
        MeterExemptionAttributeId = @MeterExemptionAttributeId
        AND MeterExemptionDetailDescription = @MeterExemptionDetailDescription
END
GO
