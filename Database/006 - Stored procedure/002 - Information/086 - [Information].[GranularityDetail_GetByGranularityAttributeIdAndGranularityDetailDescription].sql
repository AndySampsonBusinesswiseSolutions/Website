USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[GranularityDetail_GetByGranularityAttributeIdAndGranularityDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[GranularityDetail_GetByGranularityAttributeIdAndGranularityDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-17
-- Description:	Get GranularityDetail info from [Information].[GranularityDetail] table by Granularity Id and Granularity Attribute Id
-- =============================================

ALTER PROCEDURE [Information].[GranularityDetail_GetByGranularityAttributeIdAndGranularityDetailDescription]
    @GranularityAttributeId BIGINT,
    @GranularityDetailDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        GranularityDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        GranularityId,
        GranularityAttributeId,
        GranularityDetailDescription
    FROM 
        [Information].[GranularityDetail] 
    WHERE 
        GranularityAttributeId = @GranularityAttributeId
        AND GranularityDetailDescription = @GranularityDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
