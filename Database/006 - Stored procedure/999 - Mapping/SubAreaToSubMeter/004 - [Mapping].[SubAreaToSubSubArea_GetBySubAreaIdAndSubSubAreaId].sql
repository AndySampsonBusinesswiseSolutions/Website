USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[SubAreaToSubSubArea_GetBySubAreaIdAndSubSubAreaId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[SubAreaToSubSubArea_GetBySubAreaIdAndSubSubAreaId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get SubAreaToSubSubArea info from [Mapping].[SubAreaToSubSubArea] table by SubArea Id And SubArea Exemption Id
-- =============================================

ALTER PROCEDURE [Mapping].[SubAreaToSubSubArea_GetBySubAreaIdAndSubSubAreaId]
    @SubAreaId BIGINT,
    @SubSubAreaId BIGINT,
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
        SubAreaToSubSubAreaId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubAreaId,
        SubSubAreaId
    FROM 
        [Mapping].[SubAreaToSubSubArea] 
    WHERE 
        SubAreaId = @SubAreaId
        AND SubSubAreaId = @SubSubAreaId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
