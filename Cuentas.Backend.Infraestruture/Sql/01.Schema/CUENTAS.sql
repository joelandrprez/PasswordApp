USE [DB_Cuentas]
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 14/01/2023 13:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TipoCuenta_Id] [int] NULL,
	[Sitio] [varchar](1000) NULL,
	[Usuario] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Usuario_Id] [int] NULL,
	[Proyecto_Id] [int] NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCrea] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoProyecto]    Script Date: 14/01/2023 13:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoProyecto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [bit] NULL,
	[Descripcion] [varchar](50) NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCrea] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModifica] [int] NULL,
 CONSTRAINT [PK_EstadoProyecto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyecto]    Script Date: 14/01/2023 13:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyecto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[EstadoProyecto_Id] [int] NULL,
	[UsuarioCrea] [varchar](50) NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioModifica] [varchar](50) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Proyecto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoCuenta]    Script Date: 14/01/2023 13:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCuenta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[Estado] [bit] NULL,
	[UsuarioCreacion] [int] NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_TipoCuenta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 14/01/2023 13:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](50) NULL,
	[Password] [varchar](1025) NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCreacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_Proyecto] FOREIGN KEY([TipoCuenta_Id])
REFERENCES [dbo].[Proyecto] ([Id])
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Cuentas_Proyecto]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_TipoCuenta] FOREIGN KEY([TipoCuenta_Id])
REFERENCES [dbo].[TipoCuenta] ([Id])
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Cuentas_TipoCuenta]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_Usuario] FOREIGN KEY([Usuario_Id])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Cuentas_Usuario]
GO
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_EstadoProyecto] FOREIGN KEY([EstadoProyecto_Id])
REFERENCES [dbo].[EstadoProyecto] ([Id])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_EstadoProyecto]
GO
/****** Object:  StoredProcedure [dbo].[INS_RegistrarEstadoProyecto]    Script Date: 14/01/2023 13:53:20 ******/
