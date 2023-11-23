USE [reto_Guardians]
GO

/****** Object:  Table [dbo].[Relacion_Personal]    Script Date: 23/11/2023 11:38:41 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Relacion_Personal](
	[id_relacion] [int] IDENTITY(1,1) NOT NULL,
	[id_persona1] [int] NOT NULL,
	[id_persona2] [int] NOT NULL,
	[tipo_relacion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Relacion_Personal] PRIMARY KEY CLUSTERED 
(
	[id_relacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Relacion_Personal]  WITH CHECK ADD  CONSTRAINT [Primera_Persona] FOREIGN KEY([id_persona1])
REFERENCES [dbo].[Persona] ([id_persona])
GO

ALTER TABLE [dbo].[Relacion_Personal] CHECK CONSTRAINT [Primera_Persona]
GO

ALTER TABLE [dbo].[Relacion_Personal]  WITH CHECK ADD  CONSTRAINT [Segunda_Persona] FOREIGN KEY([id_persona2])
REFERENCES [dbo].[Persona] ([id_persona])
GO

ALTER TABLE [dbo].[Relacion_Personal] CHECK CONSTRAINT [Segunda_Persona]
GO

