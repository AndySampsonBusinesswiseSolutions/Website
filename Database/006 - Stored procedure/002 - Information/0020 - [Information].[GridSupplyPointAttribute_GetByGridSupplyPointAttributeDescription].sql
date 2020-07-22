USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[GridSupplyPointAttribute_GetByGridSupplyPointAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[GridSupplyPointAttribute_GetByGridSupplyPointAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get GridSupplyPointAttribute info from [Information].[GridSupplyPointAttribute] table by GridSupplyPoint Attribute Description
-- =============================================

ALTER PROCEDURE [Information].[GridSupplyPointAttribute_GetByGridSupplyPointAttributeDescription]
    @GridSupplyPointAttributeDescription VARCHAR(255),
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
        GridSupplyPointAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        GridSupplyPointAttributeDescription
    FROM 
        [Information].[GridSupplyPointAttribute] 
    WHERE 
        GridSupplyPointAttributeDescription = @GridSupplyPointAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
