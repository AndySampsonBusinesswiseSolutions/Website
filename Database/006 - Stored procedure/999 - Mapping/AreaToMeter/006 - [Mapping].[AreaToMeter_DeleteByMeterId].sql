USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[AreaToMeter_DeleteByMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[AreaToMeter_DeleteByMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-18
-- Description:	Delete AreaToMeter info from [Mapping].[AreaToMeter] table by Meter Id
-- =============================================

ALTER PROCEDURE [Mapping].[AreaToMeter_DeleteByMeterId]
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Mapping].[AreaToMeter]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        MeterId = @MeterId
        AND EffectiveToDateTime = '9999-12-31'
END
GO
