USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_Update]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessQueue_Update] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	End-date queue entry in [System].[ProcessQueue] table
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_Update]
	@GUID UNIQUEIDENTIFIER,
    @APIGUID UNIQUEIDENTIFIER,
    @HasError BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE GUID = @APIGUID)

    UPDATE
        [System].[ProcessQueue]
    SET
        EffectiveToDateTime = GETUTCDATE(),
        HasError = @HasError
    WHERE
        GUID = @GUID
        AND APIId = @APIId
END
GO
