USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToMeterTimeswitchCode_GetByMeterIdAndMeterTimeswitchCodeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToMeterTimeswitchCode_GetByMeterIdAndMeterTimeswitchCodeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get MeterToMeterTimeswitchCode info from [Mapping].[MeterToMeterTimeswitchCode] table by Meter Id And Meter Exemption Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToMeterTimeswitchCode_GetByMeterIdAndMeterTimeswitchCodeId]
    @MeterId BIGINT,
    @MeterTimeswitchCodeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        MeterToMeterTimeswitchCodeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        MeterTimeswitchCodeId
    FROM 
        [Mapping].[MeterToMeterTimeswitchCode] 
    WHERE 
        MeterId = @MeterId
        AND MeterTimeswitchCodeId = @MeterTimeswitchCodeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
