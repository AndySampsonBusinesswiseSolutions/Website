USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Get ProfileClass Detail info from [Information].[ProfileClassDetail] table by ProfileClass Attribute Id And ProfileClass Detail Description
-- =============================================

ALTER PROCEDURE [Information].[ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription]
    @ProfileClassAttributeId BIGINT,
    @ProfileClassDetailDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ProfileClassDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProfileClassId,
        ProfileClassAttributeId,
        ProfileClassDetailDescription
    FROM 
        [Information].[ProfileClassDetail] 
    WHERE 
        ProfileClassAttributeId = @ProfileClassAttributeId
        AND ProfileClassDetailDescription = @ProfileClassDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
