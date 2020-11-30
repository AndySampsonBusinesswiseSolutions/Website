USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorEntityAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorEntityAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Insert new data upload validation error sheet attribute into [Customer].[DataUploadValidationErrorEntityAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorEntityAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DataUploadValidationErrorEntityAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[DataUploadValidationErrorEntityAttribute]
    (
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorEntityAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DataUploadValidationErrorEntityAttributeDescription
    )
END
GO