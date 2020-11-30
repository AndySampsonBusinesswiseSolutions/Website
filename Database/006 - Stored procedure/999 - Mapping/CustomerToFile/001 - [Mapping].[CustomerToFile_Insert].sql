USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CustomerToFile_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CustomerToFile_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-14
-- Description:	Insert new mapping of a Customer to a File into [Mapping].[CustomerToFile] table
-- =============================================

ALTER PROCEDURE [Mapping].[CustomerToFile_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CustomerId BIGINT,
    @FileId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].CustomerToFile
    (
        CreatedByUserId,
        SourceId,
        CustomerId,
        FileId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @CustomerId,
        @FileId
    )
END
GO
