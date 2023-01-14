CREATE OR ALTER   PROC [dbo].[INS_RegistrarUsuario]
(
	@Usuario varchar(50),
	@Password varchar(1025),
	@Saltd varchar(1025),
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
	Saltd,
	FechaCreacion,
	UsuarioCreacion,
	FechaModificacion,
	UsuarioModificacion
	)
	values(
	@Usuario,
	@Password,
	@Saltd,
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
