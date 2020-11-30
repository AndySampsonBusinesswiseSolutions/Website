USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorRow_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorRow_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Insert new DataUploadValidationError detail into [Customer].[DataUploadValidationErrorRow] table
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorRow_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DataUploadValidationErrorSheetId BIGINT,
    @DataUploadValidationErrorRow BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[DataUploadValidationErrorRow]
    (
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorSheetId,
        DataUploadValidationErrorRow
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DataUploadValidationErrorSheetId,
        @DataUploadValidationErrorRow
    )
END
GO