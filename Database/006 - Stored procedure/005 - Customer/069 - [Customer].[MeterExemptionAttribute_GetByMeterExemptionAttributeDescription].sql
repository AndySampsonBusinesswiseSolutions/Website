USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get MeterExemptionAttribute info from [Customer].[MeterExemptionAttribute] table by MeterExemptionAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription]
    @MeterExemptionAttributeDescription VARCHAR(255),
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
        MeterExemptionAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterExemptionAttributeDescription
    FROM 
        [Customer].[MeterExemptionAttribute] 
    WHERE 
        MeterExemptionAttributeDescription = @MeterExemptionAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
