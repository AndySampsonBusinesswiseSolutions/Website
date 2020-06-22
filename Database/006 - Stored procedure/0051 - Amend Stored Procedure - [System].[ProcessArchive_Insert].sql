USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchive_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchive_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new ProcessArchive into [System].[ProcessArchive] table
-- =============================================

ALTER PROCEDURE [System].[ProcessArchive_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProcessArchiveGUID UNIQUEIDENTIFIER,
    @HasError BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- 2020-06-22 -> Andrew Sampson -> Added HasError
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[ProcessArchive] WHERE ProcessArchiveGUID = @ProcessArchiveGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [System].[ProcessArchive]
            (
                CreatedByUserId,
                SourceId,
                ProcessArchiveGUID,
                HasError
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProcessArchiveGUID,
                @HasError
            )
        END
END
GO
