USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[AssetDetail_GetByAssetAttributeIdAndAssetDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[AssetDetail_GetByAssetAttributeIdAndAssetDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get AssetDetail info from [Customer].[AssetDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[AssetDetail_GetByAssetAttributeIdAndAssetDetailDescription]
    @AssetAttributeId BIGINT,
    @AssetDetailDescription VARCHAR(255)
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
        AssetDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        AssetId,
        AssetAttributeId,
        AssetDetailDescription
    FROM 
        [Customer].[AssetDetail] 
    WHERE 
        AssetAttributeId = @AssetAttributeId
        AND AssetDetailDescription = @AssetDetailDescription
END
GO
