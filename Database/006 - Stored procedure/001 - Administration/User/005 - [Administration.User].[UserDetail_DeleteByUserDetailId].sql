USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[UserDetail_DeleteByUserDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Administration.User].[UserDetail_DeleteByUserDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Delete user detail from [Administration.User].[UserDetail] table
-- =============================================

ALTER PROCEDURE [Administration.User].[UserDetail_DeleteByUserDetailId]
    @UserDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Administration.User].[UserDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        UserDetailId = @UserDetailId
END
GO
