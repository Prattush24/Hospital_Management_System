
--Booking Appointments
CREATE PROCEDURE sp_BookAppointment
(
    @PatientId INT,
    @DoctorId INT,
    @AppointmentDate DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Patient validation
        IF NOT EXISTS
        (
            SELECT 1
            FROM Patients
            WHERE PatientId = @PatientId
              AND IsActive = 1
        )
            THROW 50001, 'Patient not found or inactive.', 1;

        -- Doctor validation
        IF NOT EXISTS
        (
            SELECT 1
            FROM Doctors
            WHERE DoctorId = @DoctorId
              AND IsAvailable = 1
        )
            THROW 50002, 'Doctor not found or unavailable.', 1;

        -- Appointment date validation
        IF @AppointmentDate <= GETDATE()
            THROW 50003, 'Appointment date must be in the future.', 1;

        BEGIN TRANSACTION;

            INSERT INTO Appointments
            (
                PatientId,
                DoctorId,
                AppointmentDate,
                Status
            )
            VALUES
            (
                @PatientId,
                @DoctorId,
                @AppointmentDate,
                'Scheduled'
            );

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END;
GO

CREATE PROCEDURE sp_CancelAppointment
(
    @AppointmentId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM Appointments
            WHERE AppointmentId = @AppointmentId
        )
            THROW 50004, 'Appointment not found.', 1;

        IF EXISTS
        (
            SELECT 1
            FROM Appointments
            WHERE AppointmentId = @AppointmentId
              AND Status = 'Cancelled'
        )
            THROW 50005, 'Appointment already cancelled.', 1;

        BEGIN TRANSACTION;

            UPDATE Appointments
            SET
                Status = 'Cancelled',
                CancelledAt = GETDATE()
            WHERE AppointmentId = @AppointmentId;

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END;
GO

--Get upcoming appointments
CREATE PROCEDURE sp_GetUpcomingAppointments
AS
BEGIN
    SELECT
        AppointmentId,
        PatientId,
        DoctorId,
        AppointmentDate,
        Status,
        CancelledAt
    FROM Appointments
    WHERE Status = 'Scheduled'
      AND AppointmentDate > GETDATE()
    ORDER BY AppointmentDate;
END;



--Get doctors appointments
CREATE PROCEDURE sp_GetDoctorAppointments
(
    @DoctorId INT
)
AS
BEGIN

    IF NOT EXISTS
    (
        SELECT 1
        FROM Doctors
        WHERE DoctorId = @DoctorId
    )
        THROW 50006, 'Doctor not found.', 1;

    SELECT
        AppointmentId,
        PatientId,
        DoctorId,
        AppointmentDate,
        Status,
        CancelledAt
    FROM Appointments
    WHERE DoctorId = @DoctorId
    ORDER BY AppointmentDate;
END;

--drop procedure sp_GetDoctorAppointments 


CREATE PROCEDURE sp_GetPatientAppointments
(
    @PatientId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Patients
        WHERE PatientId = @PatientId
    )
        THROW 50007, 'Patient not found.', 1;

    SELECT
        AppointmentId,
        PatientId,
        DoctorId,
        AppointmentDate,
        Status,
        CancelledAt
    FROM Appointments
    WHERE PatientId = @PatientId
    ORDER BY AppointmentDate;
END;
GO


INSERT INTO Appointments
(
    PatientId,
    DoctorId,
    AppointmentDate,
    Status,
    CancelledAt
)
VALUES
(1, 1, DATEADD(DAY, 2, GETDATE()), 'Scheduled', NULL),

(2, 1, DATEADD(DAY, 5, GETDATE()), 'Scheduled', NULL),

(3, 2, DATEADD(DAY, 1, GETDATE()), 'Scheduled', NULL),

(1, 3, DATEADD(DAY, -2, GETDATE()), 'Completed', NULL),

(2, 2, DATEADD(DAY, -5, GETDATE()), 'Completed', NULL),

(3, 3, DATEADD(DAY, 3, GETDATE()), 'Cancelled', GETDATE());