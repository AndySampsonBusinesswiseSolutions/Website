USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToMeterExemption_GetByMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToMeterExemption_GetByMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get MeterToMeterExemption info from [Mapping].[MeterToMeterExemption] table by Meter Id And Meter Exemption Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToMeterExemption_GetByMeterIdAndMeterExemptionId]
    @MeterId BIGINT,
    @MeterExemptionId BIGINT,
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
        MeterToMeterExemptionId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        MeterExemptionId
    FROM 
        [Mapping].[MeterToMeterExemption] 
    WHERE 
        MeterId = @MeterId
        AND MeterExemptionId = @MeterExemptionId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
