USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationRunDetail_GetByApplicationRunIdAndApplicationRunAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationRunDetail_GetByApplicationRunIdAndApplicationRunAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Get ApplicationRunDetail info from [System].[ApplicationRunDetail] table by ApplicationRun ID and ApplicationRun Attribute ID
-- =============================================

ALTER PROCEDURE [System].[ApplicationRunDetail_GetByApplicationRunIdAndApplicationRunAttributeId]
    @ApplicationRunID BIGINT,
    @ApplicationRunAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ApplicationRunDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ApplicationRunId,
        ApplicationRunAttributeId,
        ApplicationRunDetailDescription
    FROM 
        [System].[ApplicationRunDetail] 
    WHERE 
        ApplicationRunId = @ApplicationRunId
        AND ApplicationRunAttributeId = @ApplicationRunAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
