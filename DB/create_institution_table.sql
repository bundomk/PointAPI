CREATE TABLE [dbo].[Institution](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime] NOT NULL DEFAULT GETDATE(),
	[Description] [varchar](512) NULL,
	[Number] [varchar] (64) NULL,
	[Name] [varchar] (64) NOT NULL,
	[ResponsiblePersonName] [varchar] (32) NULL,
	[Phone] [varchar] (32) NULL,
	[Email] [varchar] (32) NULL,
	[Address] [varchar] (32) NULL,
	[City] [varchar] (16) NULL,
	[Country] [varchar] (16) NULL,
	[Longitude] [decimal](9,6) NULL,
	[Latitude] [decimal](9,6) NULL,
 CONSTRAINT [PK_Institution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO