USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get GridSupplyPoint Detail info from [Information].[GridSupplyPointDetail] table by GridSupplyPoint Type Id And GridSupplyPoint Detail Description
-- =============================================

ALTER PROCEDURE [Information].[GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription]
    @GridSupplyPointAttributeId BIGINT,
    @GridSupplyPointDetailDescription VARCHAR(255),
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
        GridSupplyPointDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        GridSupplyPointId,
        GridSupplyPointAttributeId,
        GridSupplyPointDetailDescription
    FROM 
        [Information].[GridSupplyPointDetail] 
    WHERE 
        GridSupplyPointAttributeId = @GridSupplyPointAttributeId
        AND GridSupplyPointDetailDescription = @GridSupplyPointDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
