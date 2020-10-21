USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-23
-- Description:	Get MeterExemptionAttribute info from [Information].[MeterExemptionAttribute] table by MeterExemption Attribute Description
-- =============================================

ALTER PROCEDURE [Information].[MeterExemptionAttribute_GetByMeterExemptionAttributeDescription]
    @MeterExemptionAttributeDescription VARCHAR(255),
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
        MeterExemptionAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterExemptionAttributeDescription
    FROM 
        [Information].[MeterExemptionAttribute] 
    WHERE 
        MeterExemptionAttributeDescription = @MeterExemptionAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
