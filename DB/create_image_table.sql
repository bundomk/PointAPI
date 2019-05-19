CREATE TABLE [dbo].[Image](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
	[InfoPostId] [bigint] NOT NULL,
	[Longitude] [decimal](9,6) NOT NULL,
	[Latitude] [decimal](9,6) NOT NULL,
	[TakenTime] [datetime] NULL,
	[CameraMaker] [varchar] (32) NULL,
	[CameraModel] [varchar] (32) NULL,
	[Type] [varchar] (16) NULL,
	[Path] [varchar] (128) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].Image  WITH CHECK ADD  CONSTRAINT [FK_Image_InfoPost] FOREIGN KEY([InfoPostId])
REFERENCES [dbo].[InfoPost] ([Id])
GO

ALTER TABLE [dbo].Image CHECK CONSTRAINT [FK_Image_InfoPost]
GO