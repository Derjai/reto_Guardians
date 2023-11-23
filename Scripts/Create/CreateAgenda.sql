USE [reto_Guardians]
GO

/****** Object:  Table [dbo].[Agenda]    Script Date: 23/11/2023 11:36:56 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Agenda](
	[id_evento] [int] IDENTITY(1,1) NOT NULL,
	[id_heroe] [int] NOT NULL,
	[evento] [varchar](50) NOT NULL,
	[fecha] [date] NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Agenda] PRIMARY KEY CLUSTERED 
(
	[id_evento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Agenda]  WITH CHECK ADD  CONSTRAINT [Agenda_Heroe] FOREIGN KEY([id_heroe])
REFERENCES [dbo].[Heroe] ([id_heroe])
GO

ALTER TABLE [dbo].[Agenda] CHECK CONSTRAINT [Agenda_Heroe]
GO

