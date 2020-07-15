USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[Site_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[Site_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-15
-- Description:	Insert new Site into [Temp.Customer].[Site] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[Site_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @SiteName VARCHAR(255),
    @SiteAddress VARCHAR(255),
    @SiteTown VARCHAR(255),
    @SiteCounty VARCHAR(255),
    @SitePostCode VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[Site]
    (
        ProcessQueueGUID,
        SiteName,
        SiteAddress,
        SiteTown,
        SiteCounty,
        SitePostCode
    )
    VALUES
    (
        @ProcessQueueGUID,
        @SiteName,
        @SiteAddress,
        @SiteTown,
        @SiteCounty,
        @SitePostCode
    )
END
GO
