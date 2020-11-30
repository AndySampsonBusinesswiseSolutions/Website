USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[Application_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[Application_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new Application into [System].[Application] table
-- =============================================

ALTER PROCEDURE [System].[Application_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[Application]
    (
        CreatedByUserId,
        SourceId,
        ApplicationGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationGUID
    )
END
GO