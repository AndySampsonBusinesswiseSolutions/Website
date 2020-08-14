USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get SubMeterDetail info from [Customer].[SubMeterDetail] table by SubMeter Id and SubMeter Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId]
    @SubMeterId BIGINT,
    @SubMeterAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SubMeterDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubMeterId,
        SubMeterAttributeId,
        SubMeterDetailDescription
    FROM 
        [Customer].[SubMeterDetail] 
    WHERE 
        SubMeterId = @SubMeterId
        AND SubMeterAttributeId = @SubMeterAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
