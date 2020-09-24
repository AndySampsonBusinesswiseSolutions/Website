USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[DateToForecastAgent_GetByDateId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[DateToForecastAgent_GetByDateId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-19
-- Description:	Get DateToForecastAgent info from [Mapping].[DateToForecastAgent] table by Date Id
-- =============================================

ALTER PROCEDURE [Mapping].[DateToForecastAgent_GetByDateId]
    @DateId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-19 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        DateToForecastAgentId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DateId,
        ForecastAgentId,
        Priority
    FROM 
        [Mapping].[DateToForecastAgent]
    WHERE
        DateId = @DateId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
