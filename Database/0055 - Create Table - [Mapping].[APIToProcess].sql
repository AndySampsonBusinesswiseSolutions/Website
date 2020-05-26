USE [EMaaS]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mapping].[APIToPageToProcess]') AND type in (N'U'))
DROP TABLE [Mapping].[APIToPageToProcess]
GO
CREATE TABLE [Mapping].[APIToPageToProcess]
	(
	APIToPageToProcessId bigint IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime datetime NOT NULL,
	EffectiveToDateTime datetime NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedByUserId bigint NOT NULL,
	APIId bigint NOT NULL,
	PageToProcessId bigint NOT NULL
	)  ON Mapping
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	PK_APIToPageToProcess PRIMARY KEY CLUSTERED 
	(
	APIToPageToProcessId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON Mapping

GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	DF_APIToPageToProcess_EffectiveFromDateTime DEFAULT GETDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	DF_APIToPageToProcess_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	DF_APIToPageToProcess_CreatedDateTime DEFAULT GETDATE() FOR CreatedDateTime
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	FK_APIToPageToProcess_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToPageToProcess].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToPageToProcess', N'CONSTRAINT', N'FK_APIToPageToProcess_CreatedByUserId'
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	FK_APIToPageToProcess_PageToProcessId FOREIGN KEY
	(
	PageToProcessId
	) REFERENCES [Mapping].[PageToProcess]
	(
	PageToProcessId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToPageToProcess].PageToProcessId to [Mapping].[PageToProcess].PageToProcessId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToPageToProcess', N'CONSTRAINT', N'FK_APIToPageToProcess_PageToProcessId'
GO
ALTER TABLE [Mapping].[APIToPageToProcess] ADD CONSTRAINT
	FK_APIToPageToProcess_APIId FOREIGN KEY
	(
	APIId
	) REFERENCES [System].[API]
	(
	APIId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Mapping].[APIToPageToProcess].APIId to [System].[API].APIId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Mapping', N'TABLE', N'APIToPageToProcess', N'CONSTRAINT', N'FK_APIToPageToProcess_APIId'
GO
ALTER TABLE [Mapping].[APIToPageToProcess] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
