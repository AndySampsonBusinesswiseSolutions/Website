USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorEntityAttribute_GetByDataUploadValidationErrorEntityAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorEntityAttribute_GetByDataUploadValidationErrorEntityAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Get DataUploadValidationErrorEntityAttribute info from [Customer].[DataUploadValidationErrorEntityAttribute] table by DataUploadValidationErrorEntityAttributeId
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorEntityAttribute_GetByDataUploadValidationErrorEntityAttributeId]
    @DataUploadValidationErrorEntityAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        DataUploadValidationErrorEntityAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorEntityAttributeDescription
    FROM 
        [Customer].[DataUploadValidationErrorEntityAttribute] 
    WHERE 
        DataUploadValidationErrorEntityAttributeId = @DataUploadValidationErrorEntityAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
