USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FileAttribute_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[FileAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-29
-- Description:	Insert new file attribute into [Information].[FileAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[FileAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FileAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[FileAttribute] WHERE FileAttributeDescription = @FileAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[FileAttribute]
            (
                CreatedByUserId,
                SourceId,
                FileAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FileAttributeDescription
            )
        END
END
GO
