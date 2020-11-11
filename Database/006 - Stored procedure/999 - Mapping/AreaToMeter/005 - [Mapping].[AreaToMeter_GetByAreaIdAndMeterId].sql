USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[AreaToMeter_GetByAreaIdAndMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[AreaToMeter_GetByAreaIdAndMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-11
-- Description:	Get AreaToMeter info from [Mapping].[AreaToMeter] table by Area Id and Meter Id
-- =============================================

ALTER PROCEDURE [Mapping].[AreaToMeter_GetByAreaIdAndMeterId]
    @AreaId BIGINT,
    @MeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-11-> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        AreaToMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        AreaId,
        MeterId
    FROM 
        [Mapping].[AreaToMeter]
    WHERE
        AreaId = @AreaId
        AND MeterId = @MeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
