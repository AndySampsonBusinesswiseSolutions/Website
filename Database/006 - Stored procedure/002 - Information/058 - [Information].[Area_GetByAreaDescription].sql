USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Area_GetByAreaDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Area_GetByAreaDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get Area info from [Information].[Area] table by MeterExemption Attribute Description
-- =============================================

ALTER PROCEDURE [Information].[Area_GetByAreaDescription]
    @AreaDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        AreaId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        AreaDescription
    FROM 
        [Information].[Area] 
    WHERE 
        AreaDescription = @AreaDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
