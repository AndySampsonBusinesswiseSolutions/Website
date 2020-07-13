USE [EMaaS]
GO
ALTER TABLE [Administration.User].[User] ADD CONSTRAINT
	FK_User_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Administration.User].[User].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Administration.User', N'TABLE', N'User', N'CONSTRAINT', N'FK_User_SourceId'
GO

ALTER TABLE [Administration.User].[User] ALTER COLUMN SourceId BIGINT NOT NULL
GO