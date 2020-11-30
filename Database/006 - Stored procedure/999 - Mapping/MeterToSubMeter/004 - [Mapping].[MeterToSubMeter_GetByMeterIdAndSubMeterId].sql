USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToSubMeter_GetByMeterIdAndSubMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToSubMeter_GetByMeterIdAndSubMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get MeterToSubMeter info from [Mapping].[MeterToSubMeter] table by Meter Id And Meter Exemption Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToSubMeter_GetByMeterIdAndSubMeterId]
    @MeterId BIGINT,
    @SubMeterId BIGINT,
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
        MeterToSubMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        SubMeterId
    FROM 
        [Mapping].[MeterToSubMeter] 
    WHERE 
        MeterId = @MeterId
        AND SubMeterId = @SubMeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
