USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Source_GetBySourceTypeIdAndSourceTypeEntityId]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[Source_GetBySourceTypeIdAndSourceTypeEntityId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-09
-- Description:	Get Source info from [Information].[Source] table by Source Type Id And Source Type Entity Id
-- =============================================

ALTER PROCEDURE [Information].[Source_GetBySourceTypeIdAndSourceTypeEntityId]
    @SourceTypeId BIGINT,
    @SourceTypeEntityId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SourceId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceTypeId,
        SourceTypeEntityId
    FROM 
        [Information].[Source] 
    WHERE 
        SourceTypeId = @SourceTypeId
        AND SourceTypeEntityId = @SourceTypeEntityId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
