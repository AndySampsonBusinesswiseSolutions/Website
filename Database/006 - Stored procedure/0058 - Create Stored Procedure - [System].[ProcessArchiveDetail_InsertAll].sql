USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_InsertAll]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessArchiveDetail_InsertAll] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-23
-- Description:	Insert new ProcessArchive detail into [System].[ProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_InsertAll]
    @EffectiveFromDateTime DATETIME,
    @EffectiveToDateTime DATETIME,
	@CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProcessArchiveId BIGINT,
    @ProcessArchiveAttributeId BIGINT,
    @ProcessArchiveDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[ProcessArchiveDetail] WHERE ProcessArchiveId = @ProcessArchiveId 
        AND ProcessArchiveAttributeId = @ProcessArchiveAttributeId 
        AND ProcessArchiveDetailDescription = @ProcessArchiveDetailDescription
        AND EffectiveToDateTime = @EffectiveToDateTime
        AND EffectiveFromDateTime = @EffectiveFromDateTime)
        BEGIN
            INSERT INTO [System].[ProcessArchiveDetail]
            (
                EffectiveFromDateTime,
                EffectiveToDateTime,
                CreatedByUserId,
                SourceId,
                ProcessArchiveId,
                ProcessArchiveAttributeId,
                ProcessArchiveDetailDescription
            )
            VALUES
            (
                @EffectiveFromDateTime,
                @EffectiveToDateTime,
                @CreatedByUserId,
                @SourceId,
                @ProcessArchiveId,
                @ProcessArchiveAttributeId,
                @ProcessArchiveDetailDescription
            )
        END
END
GO
