USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorSheet_GetByDataUploadValidationErrorSheetId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorSheet_GetByDataUploadValidationErrorSheetId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-11
-- Description:	Get DataUploadValidationErrorSheet info from [Customer].[DataUploadValidationErrorSheet] table by DataUploadValidationErrorSheetId
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorSheet_GetByDataUploadValidationErrorSheetId]
    @DataUploadValidationErrorSheetId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-11 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        DataUploadValidationErrorSheetId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorId,
        DataUploadValidationErrorSheetAttributeId
    FROM 
        [Customer].[DataUploadValidationErrorSheet] 
    WHERE 
        DataUploadValidationErrorSheetId = @DataUploadValidationErrorSheetId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
