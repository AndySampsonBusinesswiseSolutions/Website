USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ReferenceVolumeDetailToVolumeUnit_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ReferenceVolumeDetailToVolumeUnit_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Insert new mapping of a ReferenceVolumeDetail to a VolumeUnit into [Mapping].[ReferenceVolumeDetailToVolumeUnit] table
-- =============================================

ALTER PROCEDURE [Mapping].[ReferenceVolumeDetailToVolumeUnit_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ReferenceVolumeDetailId BIGINT,
    @VolumeUnitId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ReferenceVolumeDetailToVolumeUnit
    (
        CreatedByUserId,
        SourceId,
        ReferenceVolumeDetailId,
        VolumeUnitId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ReferenceVolumeDetailId,
        @VolumeUnitId
    )
END
GO
