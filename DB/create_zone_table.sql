CREATE TABLE [dbo].[Zone](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
	[Name] [varchar] (32) NOT NULL,
	[Description] [varchar] (128) NULL,
	[InstitutionId] [bigint] NOT NULL,
 CONSTRAINT [PK_Zone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].Zone  WITH CHECK ADD  CONSTRAINT [FK_Zone_Institution] FOREIGN KEY([InstitutionId])
REFERENCES [dbo].[Institution] ([Id])
GO

ALTER TABLE [dbo].Zone CHECK CONSTRAINT [FK_Zone_Institution]
GO