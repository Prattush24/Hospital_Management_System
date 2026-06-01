--Creating Doctors
CREATE TABLE Doctors
(
    DoctorId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL UNIQUE,
    ConsultationFee DECIMAL(10,2) NOT NULL
        CHECK (ConsultationFee >= 0),
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

drop procedure sp_AddDoctor
--Adding Doctor
CREATE OR ALTER PROCEDURE sp_AddDoctor
(
    @FullName NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @PhoneNumber VARCHAR(15),
    @ConsultationFee DECIMAL(10,2),
    @IsAvailable BIT = 1
)
AS
BEGIN

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validations

        IF @FullName IS NULL OR LTRIM(RTRIM(@FullName)) = ''
            THROW 50002, 'Doctor Name is required.', 1;

        IF @Specialization IS NULL OR LTRIM(RTRIM(@Specialization)) = ''
            THROW 50003, 'Specialization is required.', 1;

        IF @PhoneNumber IS NULL OR LTRIM(RTRIM(@PhoneNumber)) = ''
            THROW 50004, 'Phone Number is required.', 1;

        IF @ConsultationFee < 0
            THROW 50005, 'Consultation Fee cannot be negative.', 1;

        IF EXISTS (
            SELECT 1
            FROM Doctors
            WHERE PhoneNumber = @PhoneNumber
        )
            THROW 50007, 'Phone Number already exists.', 1;

        INSERT INTO Doctors
        (
            FullName,
            Specialization,
            PhoneNumber,
            ConsultationFee,
            IsAvailable,
            CreatedAt
        )
        VALUES
        (
            @FullName,
            @Specialization,
            @PhoneNumber,
            @ConsultationFee,
            @IsAvailable,
            GETDATE()
        )

        COMMIT TRANSACTION;

        SELECT 'Doctor added successfully.' AS Message;
    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END

--Get available doctors
CREATE OR ALTER PROCEDURE sp_GetAvailableDoctors
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DoctorId,
        FullName,
        Specialization,
        PhoneNumber,
        ConsultationFee
    FROM Doctors
    WHERE IsAvailable = 1;
END

--Get doctor by specialization
CREATE OR ALTER PROCEDURE sp_GetDoctorsBySpecialization
(
    @Specialization NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Specialization IS NULL
       OR LTRIM(RTRIM(@Specialization)) = ''
    BEGIN
        THROW 50008, 'Specialization is required.', 1;
    END

    SELECT
        DoctorId,
        FullName,
        Specialization,
        PhoneNumber,
        ConsultationFee,
        IsAvailable
    FROM Doctors
    WHERE Specialization = @Specialization;
END

--Inserting some values to doctors table
INSERT INTO Doctors
(
    FullName,
    Specialization,
    PhoneNumber,
    ConsultationFee,
    IsAvailable
)
VALUES
('Dr. Rajesh Kumar', 'Cardiology', '9876543210', 800.00, 1),
('Dr. Priya Sharma', 'Dermatology', '9876543211', 600.00, 1),
('Dr. Amit Verma', 'Orthopedics', '9876543212', 1000.00, 0),
('Dr. Sneha Reddy', 'Neurology', '9876543213', 1200.00, 1),
('Dr. Kiran Rao', 'Pediatrics', '9876543214', 500.00, 1);

select * from Doctors

--delete from Doctors where PhoneNumber= '9789675445'