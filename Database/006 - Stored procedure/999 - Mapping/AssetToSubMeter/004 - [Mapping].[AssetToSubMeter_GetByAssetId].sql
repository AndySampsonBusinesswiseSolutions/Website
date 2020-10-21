USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[AssetToSubMeter_GetByAssetId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[AssetToSubMeter_GetByAssetId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-19
-- Description:	Get AssetToSubMeter info from [Mapping].[AssetToSubMeter] table by SubMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[AssetToSubMeter_GetByAssetId]
    @AssetId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-19 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        AssetToSubMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        AssetId,
        SubMeterId
    FROM 
        [Mapping].[AssetToSubMeter]
    WHERE
        AssetId = @AssetId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
