USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolumeDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolumeDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new ReferenceVolume detail into [Customer].[ReferenceVolumeDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolumeDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ReferenceVolumeId BIGINT,
    @ReferenceVolumeAttributeId BIGINT,
    @ReferenceVolumeDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ReferenceVolumeDetail]
    (
        CreatedByUserId,
        SourceId,
        ReferenceVolumeId,
        ReferenceVolumeAttributeId,
        ReferenceVolumeDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ReferenceVolumeId,
        @ReferenceVolumeAttributeId,
        @ReferenceVolumeDetailDescription
    )
END
GO