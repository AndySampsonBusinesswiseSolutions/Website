USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterDetail_DeleteBySubMeterDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterDetail_DeleteBySubMeterDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Delete SubMeter detail from [Customer].[SubMeterDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterDetail_DeleteBySubMeterDetailId]
    @SubMeterDetailId BIGINT
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
        [Customer].[SubMeterDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        SubMeterDetailId = @SubMeterDetailId
END
GO
