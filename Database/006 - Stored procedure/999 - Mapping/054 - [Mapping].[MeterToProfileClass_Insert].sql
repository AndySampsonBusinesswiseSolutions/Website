USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToProfileClass_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToProfileClass_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a Meter to a ProfileClass into [Mapping].[MeterToProfileClass] table
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToProfileClass_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterId BIGINT,
    @ProfileClassId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].MeterToProfileClass
    (
        CreatedByUserId,
        SourceId,
        MeterId,
        ProfileClassId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MeterId,
        @ProfileClassId
    )
END
GO
