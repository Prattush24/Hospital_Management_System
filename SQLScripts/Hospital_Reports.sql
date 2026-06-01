--Getting appointment reports

CREATE PROCEDURE sp_GetAppointmentReport
AS
BEGIN
    SELECT
        p.PatientId,
        p.FullName AS PatientName,
        d.DoctorId,
        d.FullName AS DoctorName,
        d.Specialization,
        a.AppointmentDate,
        a.Status,
        d.ConsultationFee
    FROM Appointments a
    INNER JOIN Patients p
        ON a.PatientId = p.PatientId
    INNER JOIN Doctors d
        ON a.DoctorId = d.DoctorId
    ORDER BY a.AppointmentDate DESC;
END


--Doctors having more than two appointments
CREATE PROCEDURE sp_GetDoctorAppointmentCount
AS
BEGIN
    SELECT
        d.DoctorId,
        d.FullName AS DoctorName,
        d.Specialization,
        COUNT(d.DoctorId) as [Number of Appointments]
    FROM Appointments a
    INNER JOIN Doctors d
        ON a.DoctorId = d.DoctorId
    GROUP BY
        d.DoctoriD,
        d.FullName,
        d.Specialization
    HAVING COUNT(d.DoctorId) > 2
END

--Revenue by specialization
CREATE PROCEDURE sp_GetRevenueBySpecialization
AS
BEGIN
    SELECT
        d.Specialization,
        SUM(d.ConsultationFee) AS TotalRevenue
    FROM Appointments a
    INNER JOIN Doctors d
        ON a.DoctorId = d.DoctorId
    WHERE a.Status <> 'Cancelled'
    GROUP BY d.Specialization;
END

--Upcoming appointments for next 7 days
CREATE PROCEDURE sp_GetUpcomingAppointmentsNext7Days
AS
BEGIN
    SELECT
        p.FullName AS PatientName,
        d.FullName AS DoctorName,
        d.Specialization,
        a.AppointmentDate,
        a.Status
    FROM Appointments a
    INNER JOIN Patients p
        ON a.PatientId = p.PatientId
    INNER JOIN Doctors d
        ON a.DoctorId = d.DoctorId
    WHERE a.AppointmentDate
          BETWEEN GETDATE()
          AND DATEADD(DAY,7,GETDATE())
    ORDER BY a.AppointmentDate;
END