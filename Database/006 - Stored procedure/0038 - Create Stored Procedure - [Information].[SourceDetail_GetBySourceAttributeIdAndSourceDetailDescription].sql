USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-09
-- Description:	Get Source Detail info from [Information].[SourceDetail] table by Source Type Id And Source Detail Description
-- =============================================

ALTER PROCEDURE [Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription]
    @SourceAttributeId BIGINT,
    @SourceDetailDescription VARCHAR(255),
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
        SourceDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SourceAttributeId,
        SourceDetailDescription
    FROM 
        [Information].[SourceDetail] 
    WHERE 
        SourceAttributeId = @SourceAttributeId
        AND SourceDetailDescription = @SourceDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
