CREATE OR ALTER     PROC [dbo].[SEL_ListarCuentas]
(
	@Page			INT,
	@Size			INT,
	@Search			VARCHAR(200) = NULL,
	@OrderBy		VARCHAR(50) = 'Codigo',
	@OrderDir		VARCHAR(4) = 'ASC',
	@TotalGlobal		INT OUTPUT,
	@TotalFiltered   INT OUTPUT
)
AS
BEGIN

	DECLARE @Skip INT;
	SET @Skip = (@Size * @Page) - @Size;

	SELECT @TotalGlobal = COUNT(*) FROM Cuentas;

	SELECT @TotalFiltered = COUNT(*) from Cuentas a
	inner join TipoCuenta b on a.TipoCuenta_Id = b.id
	inner join Proyecto c on a.Proyecto_Id = c.Id
	WHERE (@Search IS NULL OR UPPER(c.Descripcion) LIKE '%' + UPPER(@Search) + '%') OR
	(@Search IS NULL OR UPPER(a.Sitio) LIKE '%' + UPPER(@Search) + '%') 

	SELECT 
	a.Id				 ,
	a.TipoCuenta_Id	 ,
	a.Sitio			 ,
	a.Usuario			 ,
	a.Usuario_Id		 ,
	a.Proyecto_Id		 ,
	a.FechaCreacion	 ,
	a.UsuarioCrea		 ,
	a.FechaModificacion,
	a.UsuarioModificacion,
	b.*,c.*
	from Cuentas a
	inner join TipoCuenta b on a.TipoCuenta_Id = b.id
	inner join Proyecto c on a.Proyecto_Id = c.Id
	WHERE (@Search IS NULL OR UPPER(c.Descripcion) LIKE '%' + UPPER(@Search) + '%') OR
	(@Search IS NULL OR UPPER(a.Sitio) LIKE '%' + UPPER(@Search) + '%') 
	ORDER BY a.FechaCreacion DESC, 
		a.FechaModificacion DESC
	OFFSET	@Skip ROWS FETCH NEXT (@Size) ROWS ONLY

END

GO

CREATE OR ALTER PROC [dbo].[SEL_BuscarCuenta]
(
	@id int
)
AS
BEGIN
	select * from Cuentas where Id=@id
END

GO

CREATE OR ALTER PROC SEL_BuscarUsuario
(
	@Usuario varchar(50)
)
AS
BEGIN

	select * from Usuario where Usuario = @Usuario
	
END
