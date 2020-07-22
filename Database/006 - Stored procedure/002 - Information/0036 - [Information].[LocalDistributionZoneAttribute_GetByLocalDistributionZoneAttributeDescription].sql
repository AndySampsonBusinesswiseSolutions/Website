USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[LocalDistributionZoneAttribute_GetByLocalDistributionZoneAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[LocalDistributionZoneAttribute_GetByLocalDistributionZoneAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get LocalDistributionZoneAttribute info from [Information].[LocalDistributionZoneAttribute] table by LocalDistributionZone Attribute Description
-- =============================================

ALTER PROCEDURE [Information].[LocalDistributionZoneAttribute_GetByLocalDistributionZoneAttributeDescription]
    @LocalDistributionZoneAttributeDescription VARCHAR(255),
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
        LocalDistributionZoneAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        LocalDistributionZoneAttributeDescription
    FROM 
        [Information].[LocalDistributionZoneAttribute] 
    WHERE 
        LocalDistributionZoneAttributeDescription = @LocalDistributionZoneAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
