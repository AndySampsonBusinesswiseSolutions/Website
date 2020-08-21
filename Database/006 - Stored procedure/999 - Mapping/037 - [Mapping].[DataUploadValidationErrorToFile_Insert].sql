USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[DataUploadValidationErrorToFile_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[DataUploadValidationErrorToFile_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new mapping of a DataUploadValidationError to a File into [Mapping].[DataUploadValidationErrorToFile] table
-- =============================================

ALTER PROCEDURE [Mapping].[DataUploadValidationErrorToFile_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DataUploadValidationErrorId BIGINT,
    @FileId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].DataUploadValidationErrorToFile
    (
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorId,
        FileId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DataUploadValidationErrorId,
        @FileId
    )
END
GO
