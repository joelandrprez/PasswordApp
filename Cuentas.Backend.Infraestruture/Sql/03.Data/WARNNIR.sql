insert into TipoCuenta
(
Descripcion,
Estado,
UsuarioCreacion,
FechaCreacion,
UsuarioModificacion,
FechaModificacion)
values
('FTP',0,1,GETDATE(),1,GETDATE()),
('Pagina Web',0,1,GETDATE(),1,GETDATE())


insert into Proyecto(
Descripcion,
EstadoProyecto_Id,
UsuarioCrea,
FechaCreacion,
UsuarioModifica,
FechaModificacion)
VALUES
('CHINALCO',1,16,GETDATE(),16,GETDATE()),
('KOMATSU',1,16,GETDATE(),16,GETDATE())