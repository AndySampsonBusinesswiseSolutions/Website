USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new DataUploadValidationError detail into [Customer].[DataUploadValidationErrorDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DataUploadValidationErrorId BIGINT,
    @DataUploadValidationErrorAttributeId BIGINT,
    @DataUploadValidationErrorDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[DataUploadValidationErrorDetail] WHERE DataUploadValidationErrorId = @DataUploadValidationErrorId 
        AND DataUploadValidationErrorAttributeId = @DataUploadValidationErrorAttributeId 
        AND DataUploadValidationErrorDetailDescription = @DataUploadValidationErrorDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[DataUploadValidationErrorDetail]
            (
                CreatedByUserId,
                SourceId,
                DataUploadValidationErrorId,
                DataUploadValidationErrorAttributeId,
                DataUploadValidationErrorDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @DataUploadValidationErrorId,
                @DataUploadValidationErrorAttributeId,
                @DataUploadValidationErrorDetailDescription
            )
        END
END
GO
