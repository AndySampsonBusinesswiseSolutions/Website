USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationRunAttribute_GetByApplicationRunAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationRunAttribute_GetByApplicationRunAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Get ApplicationRunAttribute info from [System].[ApplicationRunAttribute] table by ApplicationRunAttributeDescription
-- =============================================

ALTER PROCEDURE [System].[ApplicationRunAttribute_GetByApplicationRunAttributeDescription]
    @ApplicationRunAttributeDescription VARCHAR(255),
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
        ApplicationRunAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ApplicationRunAttributeDescription
    FROM 
        [System].[ApplicationRunAttribute] 
    WHERE 
        ApplicationRunAttributeDescription = @ApplicationRunAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
