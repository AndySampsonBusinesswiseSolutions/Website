USE [EMaaS]
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [LockUser.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ValidatePageGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [Website.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [CheckPrerequisiteAPI.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [LockUser.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [StoreLoginAttempt.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ValidateEmailAddress.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ValidateEmailAddressPasswordMapping.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ValidatePageGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ValidatePassword.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [Website.api];  
GO