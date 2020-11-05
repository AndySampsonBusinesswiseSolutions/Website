USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironmentAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironmentAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Insert new HostEnvironment attribute into [System].[HostEnvironmentAttribute] table
-- =============================================

ALTER PROCEDURE [System].[HostEnvironmentAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @HostEnvironmentAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[HostEnvironmentAttribute]
    (
        CreatedByUserId,
        SourceId,
        HostEnvironmentAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @HostEnvironmentAttributeDescription
    )
END
GO