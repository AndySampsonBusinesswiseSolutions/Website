USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[UserDetail_GetByUserDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Administration.User].[UserDetail_GetByUserDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-09
-- Description:	Get UserDetail info from [Administration.User].[UserDetail] table by User Detail Description
-- =============================================

ALTER PROCEDURE [Administration.User].[UserDetail_GetByUserDetailDescription]
    @UserDetailDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        UserDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        UserId,
        UserAttributeId,
        UserDetailDescription
    FROM 
        [Administration.User].[UserDetail] 
    WHERE 
        UserDetailDescription = @UserDetailDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
