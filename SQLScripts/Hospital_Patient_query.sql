CREATE DATABASE HospitalDB

use HospitalDB

--Creating patient table
CREATE TABLE Patients
(
    PatientId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL
        CHECK (Gender IN ('Male','Female','Other')),
    PhoneNumber NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(50) NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO


--Creating Appointments
CREATE TABLE Appointments
(
    AppointmentId INT PRIMARY KEY IDENTITY(1,1),

    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,

    AppointmentDate DATETIME NOT NULL,

    Status VARCHAR(20) NOT NULL
        DEFAULT 'Scheduled'
        CHECK (Status IN ('Scheduled','Completed','Cancelled')),

    CancelledAt DATETIME NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Appointments_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(PatientId),

    CONSTRAINT FK_Appointments_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(DoctorId)
);

--use HospitalDB
--drop table Appointments

--Register patient procedure

CREATE PROCEDURE sp_RegisterPatient
(
    @FullName NVARCHAR(100),
    @DateOfBirth DATE,
    @Gender VARCHAR(10),
    @PhoneNumber NVARCHAR(50),
    @Email NVARCHAR(50) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Full Name Validation
        IF @FullName IS NULL OR LTRIM(RTRIM(@FullName)) = ''
            THROW 50001, 'Patient name cannot be empty.', 1;

        -- Date Of Birth Validation
        IF @DateOfBirth IS NULL
            THROW 50002, 'Date of birth cannot be empty.', 1;

        IF @DateOfBirth > CAST(GETDATE() AS DATE)
            THROW 50003, 'Date of birth cannot be in the future.', 1;

        -- Gender Validation
        IF @Gender IS NULL OR LTRIM(RTRIM(@Gender)) = ''
            THROW 50004, 'Gender cannot be empty.', 1;

        IF @Gender NOT IN ('Male', 'Female', 'Other')
            THROW 50005, 'Invalid gender.', 1;

        -- Phone Validation
        IF @PhoneNumber IS NULL OR LTRIM(RTRIM(@PhoneNumber)) = ''
            THROW 50006, 'Phone number cannot be empty.', 1;

        IF EXISTS
        (
            SELECT 1
            FROM Patients
            WHERE PhoneNumber = @PhoneNumber
        )
            THROW 50007, 'Phone number already exists.', 1;

        -- Email Validation
        IF @Email IS NOT NULL
        BEGIN
            SET @Email = LTRIM(RTRIM(@Email));

            IF @Email = ''
                SET @Email = NULL;
        END

        IF @Email IS NOT NULL
           AND EXISTS
           (
               SELECT 1
               FROM Patients
               WHERE Email = @Email
           )
            THROW 50008, 'Email already exists.', 1;

        BEGIN TRANSACTION;

            INSERT INTO Patients
            (
                FullName,
                DateOfBirth,
                Gender,
                PhoneNumber,
                Email
            )
            VALUES
            (
                @FullName,
                @DateOfBirth,
                @Gender,
                @PhoneNumber,
                @Email
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



EXEC sp_RegisterPatient
    @FullName = 'Rahul Sharma',
    @DateOfBirth = '1998-05-15',
    @Gender = 'Male',
    @PhoneNumber = '9876543210',
    @Email = 'rahul@gmail.com';

USE HospitalDB;
select * from Patients


SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES;

SELECT name
FROM sys.tables;

--Update patient procedure

CREATE PROCEDURE sp_UpdatePatient
(
    @PatientId INT,
    @FullName NVARCHAR(100) = NULL,
    @DateOfBirth DATE = NULL,
    @Gender VARCHAR(10) = NULL,
    @PhoneNumber NVARCHAR(50) = NULL,
    @Email NVARCHAR(50) = NULL
)
AS
BEGIN 
  BEGIN TRY

    --Checking if patient exist
    IF NOT EXISTS
    (
        SELECT 1
        FROM Patients
        WHERE PatientId = @PatientId
    )
        THROW 50009, 'Patient not found.', 1;

    IF @FullName IS NULL
    AND @DateOfBirth IS NULL
    AND @Gender IS NULL
    AND @PhoneNumber IS NULL
    AND @Email IS NULL
    BEGIN
        THROW 50010, 'At least one field must be provided for update.', 1;
    END

    --Name validation

    IF @FullName IS NOT NULL
    AND LTRIM(RTRIM(@FullName)) = ''
        THROW 50011, 'Full name cannot be empty.', 1;

    --Date of birth
    IF @DateOfBirth IS NOT NULL
    AND @DateOfBirth > CAST(GETDATE() AS DATE)
        THROW 50003, 'Date of birth cannot be in the future.', 1;

    -- Gender Validation  
    IF @Gender IS NOT NULL
    AND @Gender NOT IN ('Male', 'Female', 'Other')
        THROW 50005, 'Invalid gender.', 1;

    -- Phone Number
    IF @PhoneNumber IS NOT NULL
    AND EXISTS
    (
        SELECT 1
        FROM Patients
        WHERE PhoneNumber = @PhoneNumber
        AND PatientId <> @PatientId
    )
        THROW 50007, 'Phone number already exists.', 1;

    --Email validation
    IF @Email IS NOT NULL
    AND EXISTS
           (
               SELECT 1
               FROM Patients
               WHERE Email = @Email
               AND PatientId <> @PatientId
           )
    THROW 50008, 'Email already exists.', 1;

BEGIN TRANSACTION;

UPDATE Patients
SET
    FullName = ISNULL(@FullName, FullName),
    DateOfBirth = ISNULL(@DateOfBirth, DateOfBirth),
    Gender = ISNULL(@Gender, Gender),
    PhoneNumber = ISNULL(@PhoneNumber, PhoneNumber),
    Email = ISNULL(@Email, Email),
    UpdatedAt = GETDATE()
WHERE PatientId = @PatientId;

COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END;

--Testing stored procedure
EXEC sp_UpdatePatient
    @PatientId = 1,
    @FullName = 'New Name';

EXEC sp_UpdatePatient
    @PatientId = 2,
    @Gender = 'ABC';

SELECT UpdatedAt
FROM Patients
WHERE PatientId = 2;
   
--Deactivating Patient
CREATE PROCEDURE sp_DeactivatePatient
(
    @PatientId INT
)
AS
BEGIN 
  BEGIN TRY
    --Checking if patient exist
    IF NOT EXISTS
    (
        SELECT 1
        FROM Patients
        WHERE PatientId = @PatientId
    )
        THROW 50009, 'Patient not found.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM Patients
        WHERE PatientId = @PatientId
          AND IsActive = 0
    )
        THROW 50010, 'Patient is already inactive.', 1;
    
BEGIN TRANSACTION;
    UPDATE Patients
    SET 
        IsActive = 0,
        UpdatedAt = GETDATE()
    WHERE 
        PatientId = @PatientId
COMMIT TRANSACTION;

  END TRY
  BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END;

--Get Active Patients
CREATE PROCEDURE sp_GetActivePatients
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
    PatientId,
    FullName,
    DateOfBirth,
    DATEDIFF(YEAR, DateOfBirth, GETDATE())
        -
        CASE
            WHEN DATEADD(YEAR,
                         DATEDIFF(YEAR, DateOfBirth, GETDATE()),
                         DateOfBirth) > GETDATE()
            THEN 1
            ELSE 0
        END AS Age,
    Gender,
    PhoneNumber, 
    Email,
    IsActive
    FROM Patients
    WHERE IsActive = 1
    ORDER BY FullName;
END;

--DROP PROCEDURE sp_GetActivePatients


--Get Patients by Id
CREATE OR ALTER PROCEDURE sp_GetPatientById
(
    @PatientId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Check if patient exists
        IF NOT EXISTS
        (
            SELECT 1
            FROM Patients
            WHERE PatientId = @PatientId
        )
        BEGIN
            THROW 50020, 'Patient not found.', 1;
        END;

        -- Return patient details
        SELECT
            PatientId,
            FullName,
            DateOfBirth,
            Gender,
            PhoneNumber,
            Email,
            IsActive,
            CreatedAt,
            UpdatedAt
        FROM Patients
        WHERE PatientId = @PatientId;

    END TRY
    BEGIN CATCH

        THROW;

    END CATCH
END;
GO

--DROP PROCEDURE sp_GetActivePatients



INSERT INTO Patients
(
    FullName,
    DateOfBirth,
    Gender,
    PhoneNumber,
    Email,
    IsActive
)
VALUES
--('Rohit Kumar', '1998-05-12', 'Male', '9123456780', 'rohit.kumar@email.com', 1),

('Anjali Gupta', '1991-08-23', 'Female', '9123456781', 'anjali.gupta@email.com', 1),

('Vikram Singh', '1984-11-15', 'Male', '9123456782', 'vikram.singh@email.com', 1),

('Pooja Sharma', '1995-02-28', 'Female', '9123456783', 'pooja.sharma@email.com', 1),

('Arjun Reddy', '2000-07-10', 'Male', '9123456784', 'arjun.reddy@email.com', 1);