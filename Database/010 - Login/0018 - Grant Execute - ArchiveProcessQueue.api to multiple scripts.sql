USE [EMaaS]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveDetail_InsertAll] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[APIToProcessArchiveDetail_Insert] TO [ArchiveProcessQueue.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByProcessQueueGUID] TO [ArchiveProcessQueue.api];  
GO