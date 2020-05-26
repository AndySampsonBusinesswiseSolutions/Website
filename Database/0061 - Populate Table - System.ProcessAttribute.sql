USE [EMaas]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM System.ProcessAttribute WHERE ProcessAttributeDescription = 'ProcessName')
    BEGIN
        INSERT INTO [System].ProcessAttribute
        (
            ProcessAttributeDescription,
            CreatedByUserId
        )
        VALUES
        ('ProcessName', 1)
    END