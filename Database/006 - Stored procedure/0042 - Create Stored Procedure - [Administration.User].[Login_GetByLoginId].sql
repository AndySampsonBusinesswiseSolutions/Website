USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[Login_GetByLoginId]'))
    BEGIN
        exec('CREATE PROCEDURE [Administration.User].[Login_GetByLoginId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-09
-- Description:	Get Login info from [Administration.User].[Login] table by Login Id
-- =============================================

ALTER PROCEDURE [Administration.User].[Login_GetByLoginId]
    @LoginId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        LoginId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        LoginSuccessful,
        ProcessArchiveGUID
    FROM 
        [Administration.User].[Login] 
    WHERE 
        LoginId = @LoginId
END
GO
