USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ApplicationToProcessArchiveDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ApplicationToProcessArchiveDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new mapping of a Application to a ProcessArchiveDetail into [Mapping].[ApplicationToProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [Mapping].[ApplicationToProcessArchiveDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationId BIGINT,
    @ProcessArchiveDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ApplicationToProcessArchiveDetail
    (
        CreatedByUserId,
        SourceId,
        ApplicationId,
        ProcessArchiveDetailId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationId,
        @ProcessArchiveDetailId
    )
END
GO
