USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[PasswordToUser_GetByPasswordIdAndUserId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get PasswordToUser info from [Mapping].[PasswordToUser_GetByPasswordIdAndUserId] table by Password Id and User Id
-- =============================================

ALTER PROCEDURE [Mapping].[PasswordToUser_GetByPasswordIdAndUserId]
    @PasswordId BIGINT,
    @UserId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        PasswordToUserId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        UserId,
        PasswordId
    FROM 
        [Mapping].[PasswordToUser]
    WHERE 
        PasswordId = @PasswordId
        AND UserId = @UserId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
