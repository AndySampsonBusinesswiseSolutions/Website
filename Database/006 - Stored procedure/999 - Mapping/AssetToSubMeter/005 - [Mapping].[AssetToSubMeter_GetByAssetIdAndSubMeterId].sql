USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[AssetToSubMeter_GetByAssetIdAndSubMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[AssetToSubMeter_GetByAssetIdAndSubMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-11
-- Description:	Get AssetToSubMeter info from [Mapping].[AssetToSubMeter] table by Asset Id and SubMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[AssetToSubMeter_GetByAssetIdAndSubMeterId]
    @AssetId BIGINT,
    @SubMeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-11 -> Andrew Sampson -> Initial development of script
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
        AND SubMeterId = @SubMeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
