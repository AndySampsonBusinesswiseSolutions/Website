USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironment_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironment_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Insert new HostEnvironment into [System].[HostEnvironment] table
-- =============================================

ALTER PROCEDURE [System].[HostEnvironment_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @HostEnvironmentGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[HostEnvironment]
    (
        CreatedByUserId,
        SourceId,
        HostEnvironmentGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @HostEnvironmentGUID
    )
END
GO