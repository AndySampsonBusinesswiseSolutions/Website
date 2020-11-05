USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIDetailToHostEnvironment_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[APIDetailToHostEnvironment_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Insert new mapping of an APIDetail to a process into [Mapping].[APIDetailToHostEnvironment] table
-- =============================================

ALTER PROCEDURE [Mapping].[APIDetailToHostEnvironment_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIDetailId BIGINT,
    @HostEnvironmentId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].APIDetailToHostEnvironment
    (
        CreatedByUserId,
        SourceId,
        APIDetailId,
        HostEnvironmentId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @APIDetailId,
        @HostEnvironmentId
    )
END
GO