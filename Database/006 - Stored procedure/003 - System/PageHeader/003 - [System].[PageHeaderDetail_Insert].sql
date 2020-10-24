USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageHeaderDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageHeaderDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-24
-- Description:	Insert new PageHeader detail into [System].[PageHeaderDetail] table
-- =============================================

ALTER PROCEDURE [System].[PageHeaderDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageHeaderId BIGINT,
    @PageHeaderAttributeId BIGINT,
    @PageHeaderDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[PageHeaderDetail]
    (
        CreatedByUserId,
        SourceId,
        PageHeaderId,
        PageHeaderAttributeId,
        PageHeaderDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageHeaderId,
        @PageHeaderAttributeId,
        @PageHeaderDetailDescription
    )
END
GO
