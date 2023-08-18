--Tabla ErrorLog
CREATE TABLE dbo.ErrorLog
(
    ErrorLogID INT IDENTITY PRIMARY KEY,
    ErrorMessage NVARCHAR(MAX),
    ErrorDate DATETIME DEFAULT GETDATE()
);

CREATE NONCLUSTERED INDEX IX_ErrorLog_ErrorDate
ON ErrorLog (ErrorDate);



CREATE TABLE dbo.Persona
(
    IdPer INT IDENTITY PRIMARY KEY,
    FRegistro DATETIME DEFAULT GETDATE(),
    FActualizacion DATETIME,
    Nombre VARCHAR(50),
    Paterno VARCHAR(50),
    Materno VARCHAR(50),
    RFC VARCHAR(13) UNIQUE, -- Índice único en RFC
    FNacimiento DATE,
    Usuario INT,
    Activo BIT DEFAULT (1)
);


-- Crear un índice único filtrado en RFC para registros activos
CREATE UNIQUE NONCLUSTERED INDEX UQ_Persona_RFC_Activo
ON dbo.Persona (RFC)
WHERE Activo = 1;



CREATE PROCEDURE [dbo].[SP_NuevaPersona]
(
    @Nombre VARCHAR(50),
    @Paterno VARCHAR(50),
    @Materno VARCHAR(50),
    @RFC VARCHAR(13),
    @FNacimiento DATE,
    @Usuario nvarchar(450),
    @Resultado INT OUTPUT,
    @MensajeError VARCHAR(150) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF LEN(@RFC) != 13
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'El RFC no es válido';
        END;
        ELSE IF EXISTS (SELECT 1 FROM dbo.Persona WHERE RFC = @RFC AND Activo = 1)
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'El RFC ya existe en el sistema';
        END;
        ELSE
        BEGIN
            INSERT INTO dbo.Persona
            (
                FRegistro,
                FActualizacion,
                Nombre,
                Paterno,
                Materno,
                RFC,
                FNacimiento,
                Usuario,
                Activo
            )
            VALUES
            (
                GETDATE(),
                NULL,
                @Nombre,
                @Paterno,
                @Materno,
                @RFC,
                @FNacimiento,
                @Usuario,
                1
            );

            SET @ID = SCOPE_IDENTITY();
            SET @Resultado = 1;
            SET @MensajeError = 'Registro exitoso';
        END;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        INSERT INTO ErrorLog (ErrorMessage) VALUES (ERROR_MESSAGE());
        SET @Resultado = 0;
        SET @MensajeError = 'Error al guardar el registro.';
    END CATCH;
END;



CREATE PROCEDURE [dbo].[SP_Actualizar]
(
    @IdPer INT,
    @Nombre VARCHAR(50),
    @Paterno VARCHAR(50),
    @Materno VARCHAR(50),
    @RFC VARCHAR(13),
    @FNacimiento DATE,
    @Usuario nvarchar(450),
    @Resultado INT OUTPUT,
    @MensajeError VARCHAR(150) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE IdPer = @IdPer)
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'La persona física no existe.';
        END
        ELSE IF EXISTS (SELECT 1 FROM dbo.Persona WHERE IdPer = @IdPer AND Activo = 0)
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'La persona ya está inactiva. No se realizó ninguna actualización.';
        END
        ELSE IF EXISTS (SELECT 1 FROM dbo.Persona WHERE RFC = @RFC )
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'No se puede Actualizar el RFC ya que entra en conflicto con otra Persona. No se realizó ninguna actualización.';
        END
        ELSE
        BEGIN
            UPDATE dbo.Persona
            SET FActualizacion = GETDATE(),
                Nombre = @Nombre,
                Paterno = @Paterno,
                Materno = @Materno,
                RFC = @RFC,
                FNacimiento = @FNacimiento,
                Usuario = @Usuario
            WHERE IdPer = @IdPer;

            SET @Resultado = 1;
            SET @MensajeError = 'Actualización exitosa';
        END;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        INSERT INTO dbo.ErrorLog (ErrorMessage, ErrorDate)
        VALUES (ERROR_MESSAGE(), GETDATE());
        SET @Resultado = 0;
        SET @MensajeError = 'Error al actualizar el registro.';
    END CATCH;
END;





CREATE PROCEDURE [dbo].[SP_Eliminar]
(
    @IdPer INT,
    @Usuario nvarchar(450),
    @Resultado INT OUTPUT,
    @MensajeError VARCHAR(150) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE IdPer = @IdPer)
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'La persona física no existe.';
        END;
        ELSE IF EXISTS (SELECT 1 FROM dbo.Persona WHERE IdPer = @IdPer AND Activo = 0)
        BEGIN
            SET @Resultado = 0;
            SET @MensajeError = 'La persona ya está inactiva. No se realizó ninguna eliminación.';
        END;
        ELSE
        BEGIN
            UPDATE dbo.Persona
            SET FActualizacion = GETDATE(),
                Activo = 0,
                Usuario = @Usuario
            WHERE IdPer = @IdPer;

            SET @Resultado = 1;
            SET @MensajeError = 'Eliminación exitosa';
        END;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        INSERT INTO dbo.ErrorLog (ErrorMessage, ErrorDate)
        VALUES (ERROR_MESSAGE(), GETDATE());
        SET @Resultado = 0;
        SET @MensajeError = 'Error al eliminar el registro.';
    END CATCH;
END;
