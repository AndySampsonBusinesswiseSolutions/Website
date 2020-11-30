USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FileDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FileDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-29
-- Description:	Insert new file detail into [Information].[FileDetail] table
-- =============================================

ALTER PROCEDURE [Information].[FileDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FileId BIGINT,
    @FileAttributeId BIGINT,
    @FileDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[FileDetail]
    (
        CreatedByUserId,
        SourceId,
        FileId,
        FileAttributeId,
        FileDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FileId,
        @FileAttributeId,
        @FileDetailDescription
    )
END
GO