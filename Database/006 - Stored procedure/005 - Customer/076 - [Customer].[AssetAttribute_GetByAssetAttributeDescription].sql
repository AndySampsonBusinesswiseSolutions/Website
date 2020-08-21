USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[AssetAttribute_GetByAssetAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[AssetAttribute_GetByAssetAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get AssetAttribute info from [Customer].[AssetAttribute] table by AssetAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[AssetAttribute_GetByAssetAttributeDescription]
    @AssetAttributeDescription VARCHAR(255),
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
        AssetAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        AssetAttributeDescription
    FROM 
        [Customer].[AssetAttribute] 
    WHERE 
        AssetAttributeDescription = @AssetAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
