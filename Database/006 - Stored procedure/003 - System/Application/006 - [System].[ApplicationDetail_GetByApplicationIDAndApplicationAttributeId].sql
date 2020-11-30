USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationDetail_GetByApplicationIdAndApplicationAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationDetail_GetByApplicationIdAndApplicationAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Get ApplicationDetail info from [System].[ApplicationDetail] table by Application ID and Application Attribute ID
-- =============================================

ALTER PROCEDURE [System].[ApplicationDetail_GetByApplicationIdAndApplicationAttributeId]
    @ApplicationID BIGINT,
    @ApplicationAttributeId BIGINT,
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
        ApplicationDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ApplicationId,
        ApplicationAttributeId,
        ApplicationDetailDescription
    FROM 
        [System].[ApplicationDetail] 
    WHERE 
        ApplicationId = @ApplicationId
        AND ApplicationAttributeId = @ApplicationAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
