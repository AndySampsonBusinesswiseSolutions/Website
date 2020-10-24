USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationError_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationError_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new DataUploadValidationError into [Customer].[DataUploadValidationError] table
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationError_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DataUploadValidationErrorGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[DataUploadValidationError]
    (
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DataUploadValidationErrorGUID
    )
END
GO