USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToReferenceVolume_GetByContractIdAndReferenceVolumeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToReferenceVolume_GetByContractIdAndReferenceVolumeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-12
-- Description:	Get ContractToReferenceVolume info from [Mapping].[ContractToReferenceVolume] table by Contract Id and ReferenceVolume Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToReferenceVolume_GetByContractIdAndReferenceVolumeId]
    @ContractId BIGINT,
    @ReferenceVolumeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-12 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ContractToReferenceVolumeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractId,
        ReferenceVolumeId
    FROM 
        [Mapping].[ContractToReferenceVolume]
    WHERE 
        ContractId = @ContractId
        AND ReferenceVolumeId = @ReferenceVolumeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
