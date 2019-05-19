CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
	[Key] uniqueidentifier NOT NULL DEFAULT NEWID(),
	[Description] [varchar](512) NULL,
	[DeviceId] [varchar] (64) NOT NULL,
	[FirstName] [varchar] (64) NOT NULL,
	[LastName] [varchar] (64) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE UNIQUE INDEX IX_User_DeviceId
ON [User] (DeviceId)
GO

CREATE NONCLUSTERED INDEX IX_User_Key
ON [User] ([Key])
GO