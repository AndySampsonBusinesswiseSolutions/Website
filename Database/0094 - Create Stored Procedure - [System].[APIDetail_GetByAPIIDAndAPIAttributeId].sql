USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[APIDetail_GetByAPIIDAndAPIAttributeId]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[APIDetail_GetByAPIIDAndAPIAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get APIDetail info from [System].[APIDetail] table by API ID and API Attribute ID
-- =============================================

ALTER PROCEDURE [System].[APIDetail_GetByAPIIDAndAPIAttributeId]
    @APIID BIGINT,
    @APIAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        APIDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        APIId,
        APIAttributeId,
        APIDetailDescription
    FROM 
        [System].[APIDetail] 
    WHERE 
        APIId = @APIId
        AND APIAttributeId = @APIAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
