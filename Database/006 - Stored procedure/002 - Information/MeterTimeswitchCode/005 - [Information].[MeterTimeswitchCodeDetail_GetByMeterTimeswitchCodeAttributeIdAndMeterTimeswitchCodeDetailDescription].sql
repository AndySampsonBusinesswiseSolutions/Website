USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get MeterTimeswitchCode Detail info from [Information].[MeterTimeswitchCodeDetail] table by MeterTimeswitchCode Attribute Id And MeterTimeswitchCode Detail Description
-- =============================================

ALTER PROCEDURE [Information].[MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription]
    @MeterTimeswitchCodeAttributeId BIGINT,
    @MeterTimeswitchCodeDetailDescription VARCHAR(255),
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
        MeterTimeswitchCodeDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterTimeswitchCodeId,
        MeterTimeswitchCodeAttributeId,
        MeterTimeswitchCodeDetailDescription
    FROM 
        [Information].[MeterTimeswitchCodeDetail] 
    WHERE 
        MeterTimeswitchCodeAttributeId = @MeterTimeswitchCodeAttributeId
        AND MeterTimeswitchCodeDetailDescription = @MeterTimeswitchCodeDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
