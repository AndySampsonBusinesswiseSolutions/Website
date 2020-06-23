USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIToProcessArchiveDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[APIToProcessArchiveDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-23
-- Description:	Insert new mapping of a API to a ProcessArchiveDetail into [Mapping].[APIToProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [Mapping].[APIToProcessArchiveDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIId BIGINT,
    @ProcessArchiveDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].APIToProcessArchiveDetail
    (
        CreatedByUserId,
        SourceId,
        APIId,
        ProcessArchiveDetailId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @APIId,
        @ProcessArchiveDetailId
    )
END
GO
