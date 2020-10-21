USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorRow_GetByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorRow_GetByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Get DataUploadValidationErrorRow info from [Customer].[DataUploadValidationErrorRow] table by DataUploadValidationErrorSheetId and DataUploadValidationErrorRow
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorRow_GetByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow]
    @DataUploadValidationErrorSheetId BIGINT,
    @DataUploadValidationErrorRow BIGINT,
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
        DataUploadValidationErrorRowId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorSheetId,
        DataUploadValidationErrorRow
    FROM 
        [Customer].[DataUploadValidationErrorRow] 
    WHERE 
        DataUploadValidationErrorSheetId = @DataUploadValidationErrorSheetId
        AND DataUploadValidationErrorRow = @DataUploadValidationErrorRow
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
