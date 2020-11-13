USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[GridSupplyPointToMeter_GetByGridSupplyPointIdAndMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[GridSupplyPointToMeter_GetByGridSupplyPointIdAndMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-12
-- Description:	Get GridSupplyPointToMeter info from [Mapping].[GridSupplyPointToMeter] table by GridSupplyPoint Id and Meter Id
-- =============================================

ALTER PROCEDURE [Mapping].[GridSupplyPointToMeter_GetByGridSupplyPointIdAndMeterId]
    @GridSupplyPointId BIGINT,
    @MeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-12 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        GridSupplyPointToMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        GridSupplyPointId,
        MeterId
    FROM 
        [Mapping].[GridSupplyPointToMeter]
    WHERE 
        GridSupplyPointId = @GridSupplyPointId
        AND MeterId = @MeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
