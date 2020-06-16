USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get ProcessArchiveAttribute info from [System].[ProcessArchiveAttribute] table by ProcessArchiveAttributeDescription
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription]
    @ProcessArchiveAttributeDescription VARCHAR(255),
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
        ProcessArchiveAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProcessArchiveAttributeDescription
    FROM 
        [System].[ProcessArchiveAttribute] 
    WHERE 
        ProcessArchiveAttributeDescription = @ProcessArchiveAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
