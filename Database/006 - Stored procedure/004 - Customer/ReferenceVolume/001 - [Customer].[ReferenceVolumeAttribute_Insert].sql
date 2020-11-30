USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolumeAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolumeAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Insert new reference volume attribute into [Customer].[ReferenceVolumeAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolumeAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ReferenceVolumeAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ReferenceVolumeAttribute]
    (
        CreatedByUserId,
        SourceId,
        ReferenceVolumeAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ReferenceVolumeAttributeDescription
    )
END
GO