CREATE TABLE [dbo].InfoPost(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
	[Description] [nvarchar](512) NULL,
	[UserId] [bigint] NOT NULL,
	[Longitude] [decimal](9,6) NOT NULL,
	[Latitude] [decimal](9,6) NOT NULL,
	[IsApproved] [bit] NULL,
	[ApprovedTime] [datetime] NULL,
	[FixedBy] [bigint] NULL,
	[BelongTo] [bigint] NULL,
	[FixedTime] [datetime] NULL,
 CONSTRAINT [PK_InfoPost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].InfoPost  WITH CHECK ADD  CONSTRAINT [FK_InfoPost_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].InfoPost CHECK CONSTRAINT [FK_InfoPost_User]
GO

ALTER TABLE [dbo].InfoPost  WITH CHECK ADD  CONSTRAINT [FK_InfoPost_FixedInstitution] FOREIGN KEY([FixedBy])
REFERENCES [dbo].[Institution] ([Id])
GO

ALTER TABLE [dbo].InfoPost CHECK CONSTRAINT [FK_InfoPost_FixedInstitution]
GO

ALTER TABLE [dbo].InfoPost  WITH CHECK ADD  CONSTRAINT [FK_InfoPost_BelongToInstitution] FOREIGN KEY([BelongTo])
REFERENCES [dbo].[Institution] ([Id])
GO

ALTER TABLE [dbo].InfoPost CHECK CONSTRAINT [FK_InfoPost_BelongToInstitution]
GO