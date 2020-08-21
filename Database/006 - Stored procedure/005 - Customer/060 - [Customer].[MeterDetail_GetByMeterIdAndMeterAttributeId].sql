USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterDetail_GetByMeterIdAndMeterAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterDetail_GetByMeterIdAndMeterAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get MeterDetail info from [Customer].[MeterDetail] table by Meter Id and Meter Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[MeterDetail_GetByMeterIdAndMeterAttributeId]
    @MeterId BIGINT,
    @MeterAttributeId BIGINT,
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
        MeterDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        MeterAttributeId,
        MeterDetailDescription
    FROM 
        [Customer].[MeterDetail] 
    WHERE 
        MeterId = @MeterId
        AND MeterAttributeId = @MeterAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
