USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[SubAreaToSubMeter_GetBySubAreaIdAndSubMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[SubAreaToSubMeter_GetBySubAreaIdAndSubMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get SubAreaToSubMeter info from [Mapping].[SubAreaToSubMeter] table by SubArea Id And SubMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[SubAreaToSubMeter_GetBySubAreaIdAndSubMeterId]
    @SubAreaId BIGINT,
    @SubMeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SubAreaToSubMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubAreaId,
        SubMeterId
    FROM 
        [Mapping].[SubAreaToSubMeter] 
    WHERE 
        SubAreaId = @SubAreaId
        AND SubMeterId = @SubMeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
