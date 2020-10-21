USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToReferenceVolume_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToReferenceVolume_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Insert new mapping of a Contract to a ReferenceVolume into [Mapping].[ContractToReferenceVolume] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToReferenceVolume_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractId BIGINT,
    @ReferenceVolumeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractToReferenceVolume
    (
        CreatedByUserId,
        SourceId,
        ContractId,
        ReferenceVolumeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractId,
        @ReferenceVolumeId
    )
END
GO
