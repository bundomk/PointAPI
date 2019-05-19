CREATE TABLE [dbo].[ZonePoint](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ZoneId] [bigint] NOT NULL,
	[Longitude] [decimal](9,6) NOT NULL,
	[Latitude] [decimal](9,6) NOT NULL,
 CONSTRAINT [PK_ZonePoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].ZonePoint  WITH CHECK ADD  CONSTRAINT [FK_ZonePoint_Zone] FOREIGN KEY([ZoneId])
REFERENCES [dbo].[Zone] ([Id])
GO

ALTER TABLE [dbo].ZonePoint CHECK CONSTRAINT [FK_ZonePoint_Zone]
GO