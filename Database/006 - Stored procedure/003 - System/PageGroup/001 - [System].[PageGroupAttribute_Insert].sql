USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageGroupAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageGroupAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-24
-- Description:	Insert new PageGroup attribute into [System].[PageGroupAttribute] table
-- =============================================

ALTER PROCEDURE [System].[PageGroupAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageGroupAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[PageGroupAttribute]
    (
        CreatedByUserId,
        SourceId,
        PageGroupAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageGroupAttributeDescription
    )
END
GO
