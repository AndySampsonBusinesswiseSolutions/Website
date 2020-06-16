USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[User_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Administration.User].[User_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user into [Administration.User].[User] table
-- =============================================

ALTER PROCEDURE [Administration.User].[User_Insert]
    @UserGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[User] WHERE GUID = @UserGUID)
        BEGIN
            INSERT INTO [Administration.User].[User]
            (
                GUID
            )
            VALUES
            (
                @UserGUID
            )
        END
END
GO
