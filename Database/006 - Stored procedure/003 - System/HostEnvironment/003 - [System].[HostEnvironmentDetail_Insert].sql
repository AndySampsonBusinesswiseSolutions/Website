USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironmentDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironmentDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Insert new HostEnvironment detail into [System].[HostEnvironmentDetail] table
-- =============================================

ALTER PROCEDURE [System].[HostEnvironmentDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @HostEnvironmentId BIGINT,
    @HostEnvironmentAttributeId BIGINT,
    @HostEnvironmentDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[HostEnvironmentDetail]
    (
        CreatedByUserId,
        SourceId,
        HostEnvironmentId,
        HostEnvironmentAttributeId,
        HostEnvironmentDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @HostEnvironmentId,
        @HostEnvironmentAttributeId,
        @HostEnvironmentDetailDescription
    )
END
GO