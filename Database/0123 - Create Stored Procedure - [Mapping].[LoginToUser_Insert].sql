USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[LoginToUser_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[LoginToUser_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new mapping of a Login to a User into [Mapping].[LoginToUser] table
-- =============================================

ALTER PROCEDURE [Mapping].[LoginToUser_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @LoginId BIGINT,
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

    INSERT INTO [Mapping].LoginToUser
    (
        CreatedByUserId,
        SourceId,
        LoginId,
        UserId
    )
    VALUES
    (
        @UserId,
        @SourceId,
        @LoginId,
        @UserId
    )
END
GO
