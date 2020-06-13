USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[LoginToUser_GetByUserId]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[LoginToUser_GetByUserId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get LoginToUser info from [Mapping].[LoginToUser_GetByUserId] table by User Id
-- =============================================

ALTER PROCEDURE [Mapping].[LoginToUser_GetByUserId]
    @UserId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        LoginToUserId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        UserId,
        LoginId
    FROM 
        [Mapping].[LoginToUser]
    WHERE 
        UserId = @UserId
END
GO
