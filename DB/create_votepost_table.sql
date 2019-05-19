CREATE TABLE [dbo].[VotePost](
	[UserId] [bigint] NOT NULL,
	[InfoPostId] [bigint] NOT NULL,
	[Vote] [bit] NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
 CONSTRAINT [PK_VotePost] PRIMARY KEY CLUSTERED 
(
	[InfoPostId] ASC, [UserId] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].VotePost  WITH CHECK ADD  CONSTRAINT [FK_VotePost_InfoPost] FOREIGN KEY([InfoPostId])
REFERENCES [dbo].[InfoPost] ([Id])
GO

ALTER TABLE [dbo].VotePost CHECK CONSTRAINT [FK_VotePost_InfoPost]
GO

ALTER TABLE [dbo].VotePost  WITH CHECK ADD  CONSTRAINT [FK_VotePost_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].VotePost CHECK CONSTRAINT [FK_VotePost_User]
GO