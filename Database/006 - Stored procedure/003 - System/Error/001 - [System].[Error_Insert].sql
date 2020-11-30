USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[Error_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[Error_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-22
-- Description:	Insert new Error into [System].[Error] table
-- =============================================

ALTER PROCEDURE [System].[Error_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ErrorGUID UNIQUEIDENTIFIER,
    @ErrorMessage VARCHAR(255),
	@ErrorType VARCHAR(255),
	@ErrorSource VARCHAR(MAX)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[Error]
    (
        CreatedByUserId,
        SourceId,
        ErrorGUID,
        ErrorMessage,
        ErrorType,
        ErrorSource
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ErrorGUID,
        @ErrorMessage,
        @ErrorType,
        @ErrorSource
    )
END
GO