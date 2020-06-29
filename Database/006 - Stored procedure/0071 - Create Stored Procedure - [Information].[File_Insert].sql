USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[File_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[File_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-29
-- Description:	Insert new file into [Information].[File] table
-- =============================================

ALTER PROCEDURE [Information].[File_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FileGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[File] WHERE FileGUID = @FileGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[File]
            (
                CreatedByUserId,
                SourceId,
                FileGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FileGUID
            )
        END
END
GO
