CREATE TABLE Menu(
	[Id] [int] NOT NULL,
	[Nivel] [int] NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
	[Url] [varchar](500) NOT NULL,
	[MenuPadreId] [int] NULL,
	[Icono] [varchar](50) NOT NULL,
	[ColorFondo] [char](7) NOT NULL,
	[ColorTexto] [char](7) NOT NULL,
	[ColorIcono] [varchar](7) NOT NULL,
	[Orden] [int] NOT NULL,
	[Habilitado] [bit] NOT NULL,
	[Creacion] [datetime] NOT NULL,
	[Crea] [varchar](50) NOT NULL,
	[Modificacion] [datetime] NULL,
	[Modifica] [varchar](50) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
