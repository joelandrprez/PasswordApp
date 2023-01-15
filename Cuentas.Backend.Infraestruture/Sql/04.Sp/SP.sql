CREATE OR ALTER     PROC [dbo].[INS_RegistrarUsuario]
(
	@Usuario varchar(50),
	@Password varchar(1025),
	@FechaCreacion datetime,
	@UsuarioCreacion varchar(50),
	@FechaModificacion datetime,
	@UsuarioModificacion varchar(50)

)
AS
BEGIN
	insert into Usuario
	(
	Usuario,
	Password,
	FechaCreacion,
	UsuarioCreacion,
	FechaModificacion,
	UsuarioModificacion
	)
	values(
	@Usuario,
	@Password,
	@FechaCreacion,
	@UsuarioCreacion,
	@FechaModificacion,
	@UsuarioModificacion
	)
END


GO

CREATE OR ALTER PROC SEL_BuscarCuenta
(
	@Usuario varchar(50)
)
AS
BEGIN
	select top 1 * from Usuario where Usuario = @Usuario
END

GO

CREATE OR ALTER PROC UPD_ActualizarUsuario
(
	@Id int,
	@Password varchar(1025),
	@FechaModificacion datetime,
	@UsuarioModificacion varchar(50)
)
AS
BEGIN

	update Usuario 
	set
	Password		   = @Password,
	FechaModificacion  = @FechaModificacion,
	UsuarioModificacion= @UsuarioModificacion
	where Id=@Id
END

GO

CREATE OR ALTER PROC SEL_ListarEstadoEstadoProyecto
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

	SELECT @TotalGlobal = COUNT(*) FROM EstadoProyecto;

	SELECT @TotalFiltered = COUNT(*) 
	FROM EstadoProyecto
	WHERE (@Search IS NULL OR UPPER(Descripcion) LIKE '%' + UPPER(@Search) + '%') 

	SELECT *	
	FROM EstadoProyecto
	WHERE (@Search IS NULL OR UPPER(Descripcion) LIKE '%' + UPPER(@Search) + '%') 
	ORDER BY FechaCreacion DESC, 
		FechaModificacion DESC
	OFFSET	@Skip ROWS FETCH NEXT (@Size) ROWS ONLY

END

GO

CREATE OR ALTER PROC INS_RegistrarEstadoProyecto
(
	@Estado bit,
	@Descripcion varchar(50),
	@FechaCreacion datetime,
	@UsuarioCrea int,
	@FechaModificacion datetime,
	@UsuarioModifica int
)
AS
BEGIN

	insert into EstadoProyecto
	(
	Estado,
	Descripcion,
	FechaCreacion,
	UsuarioCrea,
	FechaModificacion,
	UsuarioModifica)
	values(
	@Estado,
	@Descripcion,
	@FechaCreacion,
	@UsuarioCrea,
	@FechaModificacion,
	@UsuarioModifica
	)

END

GO

CREATE OR ALTER PROC UPD_ActualizarEstadoProyecto
(
	@Id int,
	@Estado bit,
	@Descripcion varchar(50),
	@FechaModificacion datetime,
	@UsuarioModifica int
)
AS
BEGIN

	update EstadoProyecto 
	set 
	Estado			 =@Estado,			 
	Descripcion		 =@Descripcion,		 
	FechaModificacion=@FechaModificacion,
	UsuarioModifica	 =@UsuarioModifica	 
	where Id =@Id
END

GO

CREATE OR ALTER PROC INS_RegistrarCuentas
(
@TipoCuenta_Id	int,
@Sitio	varchar(1000),
@Usuario	varchar(50),
@Password	varchar(50),
@Usuario_Id	int,
@Proyecto_Id	int,
@FechaCreacion	datetime,
@UsuarioCrea	int,
@FechaModificacion	datetime,
@UsuarioModificacion	int
)
AS
BEGIN
	insert into cuentas
	(TipoCuenta_Id,
	Sitio,
	Usuario,
	Password,
	Usuario_Id,
	Proyecto_Id,
	FechaCreacion,
	UsuarioCrea,
	FechaModificacion,
	UsuarioModificacion)
	Values(
	@TipoCuenta_Id, 
	@Sitio,
	@Usuario,
	@Password,
	@Usuario_Id,
	@Proyecto_Id,
	@FechaCreacion,
	@UsuarioCrea,
	@FechaModificacion,
	@UsuarioModificacion	
	)
END

GO

CREATE OR ALTER PROC UPD_UpdateCuentas
(
@Id	int,
@TipoCuenta_Id	int,
@Sitio	varchar(1000),
@Usuario	varchar(50),
@Password	varchar(50),
@Usuario_Id	int,
@Proyecto_Id	int,
@FechaModificacion	datetime,
@UsuarioModificacion	int
)
AS
BEGIN
	update cuentas set 
	TipoCuenta_Id=@TipoCuenta_Id, 
	Sitio=@Sitio,
	Usuario=@Usuario,
	Password=@Password,
	Usuario_Id=@Usuario_Id,
	Proyecto_Id=@Proyecto_Id,
	FechaModificacion=@FechaModificacion,
	UsuarioModificacion=@UsuarioModificacion	
	where id = @Id
END

GO

CREATE OR ALTER   PROC SEL_ListarCuentas
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

	SELECT * from Cuentas a
	inner join TipoCuenta b on a.TipoCuenta_Id = b.id
	inner join Proyecto c on a.Proyecto_Id = c.Id
	WHERE (@Search IS NULL OR UPPER(c.Descripcion) LIKE '%' + UPPER(@Search) + '%') OR
	(@Search IS NULL OR UPPER(a.Sitio) LIKE '%' + UPPER(@Search) + '%') 
	ORDER BY a.FechaCreacion DESC, 
		a.FechaModificacion DESC
	OFFSET	@Skip ROWS FETCH NEXT (@Size) ROWS ONLY

END

GO

CREATE OR ALTER   PROC SEL_ListarUsuarios
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

	SELECT @TotalGlobal = COUNT(*) FROM Usuario;

	SELECT @TotalFiltered = COUNT(*) from Usuario
	WHERE (@Search IS NULL OR UPPER(Usuario) LIKE '%' + UPPER(@Search) + '%') 

	SELECT * from Usuario
	WHERE (@Search IS NULL OR UPPER(Usuario) LIKE '%' + UPPER(@Search) + '%') 
	ORDER BY FechaCreacion DESC, 
		FechaModificacion DESC
	OFFSET	@Skip ROWS FETCH NEXT (@Size) ROWS ONLY

END

GO

CREATE OR ALTER   PROC SEL_ListarProyectos
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

	SELECT @TotalGlobal = COUNT(*) FROM Proyecto;

	SELECT @TotalFiltered = COUNT(*) from Proyecto
	WHERE (@Search IS NULL OR UPPER(Descripcion) LIKE '%' + UPPER(@Search) + '%') 

	SELECT * from Proyecto
	WHERE (@Search IS NULL OR UPPER(Descripcion) LIKE '%' + UPPER(@Search) + '%') 
	ORDER BY FechaCreacion DESC, 
		FechaModificacion DESC
	OFFSET	@Skip ROWS FETCH NEXT (@Size) ROWS ONLY

END

GO

CREATE OR ALTER PROC INS_RegistrarProyecto
(
@Descripcion	varchar(50),
@EstadoProyecto_Id	int,
@UsuarioCrea	int,
@FechaCreacion	datetime,
@UsuarioModifica	int,
@FechaModificacion	datetime
)
AS
BEGIN
	insert into Proyecto
	(
	Descripcion,
	EstadoProyecto_Id,
	UsuarioCrea,
	FechaCreacion,
	UsuarioModifica,
	FechaModificacion
	)values
	(
	@Descripcion,
	@EstadoProyecto_Id,
	@UsuarioCrea,
	@FechaCreacion,
	@UsuarioModifica,
	@FechaModificacion	
	)
END

GO

CREATE OR ALTER PROC UDP_ActualizarProyecto
(
@Id	int,
@Descripcion	varchar(50),
@EstadoProyecto_Id	int,
@UsuarioModifica	int,
@FechaModificacion	datetime
)
AS
BEGIN
	update Proyecto set 
	Descripcion = @Descripcion,
	EstadoProyecto_Id	= @EstadoProyecto_Id,
	UsuarioModifica	= @UsuarioModifica,
	FechaModificacion = @FechaModificacion	
	where id = @Id
END