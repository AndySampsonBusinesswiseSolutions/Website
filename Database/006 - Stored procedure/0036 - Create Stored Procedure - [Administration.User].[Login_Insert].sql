USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[Login_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Administration.User].[Login_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new Login attempt into [Administration.User].[Login] table
-- =============================================

ALTER PROCEDURE [Administration.User].[Login_Insert]
	@UserId BIGINT,
    @SourceId BIGINT,
    @LoginSuccessful BIT,
    @ProcessArchiveGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Administration.User].[Login]
    (
        CreatedByUserId,
        SourceId,
        LoginSuccessful,
        ProcessArchiveGUID
    )
    VALUES
    (
        @UserId,
        @SourceId,
        @LoginSuccessful,
        @ProcessArchiveGUID
    )
END
GO
