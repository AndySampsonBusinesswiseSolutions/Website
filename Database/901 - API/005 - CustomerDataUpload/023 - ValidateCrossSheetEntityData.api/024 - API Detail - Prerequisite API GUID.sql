USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'E78B2351-B1E6-464F-8D90-3793FACDABF4')
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Prerequisite API GUID')

EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '535FDE7A-8720-4B72-BF68-DAC8FB95FBE9' --ValidateSiteData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '1DDDA8F8-F996-4B08-A28A-19F4FB0C922D' --ValidateMeterData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '35FA9EE3-77EC-4D1D-B622-443D28DA1608' --ValidateSubMeterData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '77B71231-880F-4470-83C9-0ED845BDDDCA' --ValidateMeterUsageData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '4E754A46-5F17-47BF-9D40-C7A95412EEFB' --ValidateSubMeterUsageData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '8253F798-A8F4-404D-B2BC-5DC87EFE839B' --ValidateCustomerData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '0BA4C8E7-3723-4106-A117-656F6871BC99' --ValidateMeterExemptionData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'B8F5A9D3-CD9F-44F2-B3EA-DCECA0F7CCFF' --ValidateFixedContractData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'BE59D94B-61BE-4900-B336-36AEB8904973' --ValidateFlexContractData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'D520F4DC-F582-45D7-A53C-608156991C6E' --ValidateFlexReferenceVolumeData.api
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'AFFEE0C0-D660-4859-A7E9-93C3F2BC5492' --ValidateFlexTradeData.api