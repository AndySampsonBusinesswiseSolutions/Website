USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterDetail_DeleteByMeterDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterDetail_DeleteByMeterDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Delete Meter detail from [Customer].[MeterDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[MeterDetail_DeleteByMeterDetailId]
    @MeterDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Customer].[MeterDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        MeterDetailId = @MeterDetailId
END
GO
