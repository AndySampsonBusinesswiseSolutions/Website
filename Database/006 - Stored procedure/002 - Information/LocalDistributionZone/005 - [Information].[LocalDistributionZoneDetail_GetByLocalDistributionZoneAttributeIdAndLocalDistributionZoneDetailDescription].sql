USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get LocalDistributionZone Detail info from [Information].[LocalDistributionZoneDetail] table by LocalDistributionZone Attribute Id And LocalDistributionZone Detail Description
-- =============================================

ALTER PROCEDURE [Information].[LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription]
    @LocalDistributionZoneAttributeId BIGINT,
    @LocalDistributionZoneDetailDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        LocalDistributionZoneDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        LocalDistributionZoneId,
        LocalDistributionZoneAttributeId,
        LocalDistributionZoneDetailDescription
    FROM 
        [Information].[LocalDistributionZoneDetail] 
    WHERE 
        LocalDistributionZoneAttributeId = @LocalDistributionZoneAttributeId
        AND LocalDistributionZoneDetailDescription = @LocalDistributionZoneDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
