USE [master]
GO

CREATE DATABASE [producto]
GO
USE [producto]
GO
CREATE TABLE [dbo].[marca](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_marca] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[producto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[detalle] [varchar](200) NOT NULL,	
	[tipo] [int] NOT NULL,
	[IdMarca] [int] NOT NULL,
	[Precio] [float] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	CONSTRAINT fk_marca FOREIGN KEY (IdMarca) REFERENCES marca (Id),
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[marca] ON 

INSERT [dbo].[marca] ([Id], [Nombre]) VALUES (1, N'ASUS')
INSERT [dbo].[marca] ([Id], [Nombre]) VALUES (2, N'HP')
INSERT [dbo].[marca] ([Id], [Nombre]) VALUES (3, N'BANGHO')
INSERT [dbo].[marca] ([Id], [Nombre]) VALUES (4, N'BGH')
SET IDENTITY_INSERT [dbo].[marca] OFF
GO
SET IDENTITY_INSERT [dbo].[Producto] ON 

INSERT [dbo].[Producto] ([Id], [detalle], [tipo], [IdMarca], [Precio], [Fecha]) VALUES (1, N'Ideal para oficina', 1, 1, 150000, '02/10/2020')
INSERT [dbo].[Producto] ([Id], [detalle], [tipo], [IdMarca], [Precio], [Fecha]) VALUES (2, N'Ideal para la casa', 2, 2, 750.3, '02/01/2021')
INSERT [dbo].[Producto] ([Id], [detalle], [tipo], [IdMarca], [Precio], [Fecha]) VALUES (3, N'Ideal para niños', 1, 3, 1580.75, '06/11/2020')
INSERT [dbo].[Producto] ([Id], [detalle], [tipo], [IdMarca], [Precio], [Fecha]) VALUES (4, N'Ideal para Programadores', 2, 4, 6800, '12/10/2018')
SET IDENTITY_INSERT [dbo].[Producto] OFF
GO
USE [master]
GO
ALTER DATABASE [producto] SET  READ_WRITE 
GO
