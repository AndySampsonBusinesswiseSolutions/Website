USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorSheetAttribute_GetByDataUploadValidationErrorSheetAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorSheetAttribute_GetByDataUploadValidationErrorSheetAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Get DataUploadValidationErrorSheetAttribute info from [Customer].[DataUploadValidationErrorSheetAttribute] table by DataUploadValidationErrorSheetAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorSheetAttribute_GetByDataUploadValidationErrorSheetAttributeDescription]
    @DataUploadValidationErrorSheetAttributeDescription VARCHAR(255),
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
        DataUploadValidationErrorSheetAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorSheetAttributeDescription
    FROM 
        [Customer].[DataUploadValidationErrorSheetAttribute] 
    WHERE 
        DataUploadValidationErrorSheetAttributeDescription = @DataUploadValidationErrorSheetAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
