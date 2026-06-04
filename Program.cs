using hospital.Data;
using hospital.Middleware;
using hospital.Models;
using hospital.Repository;
using hospital.Repository.Interface;
using hospital.Services;
using hospital.Services.Interface;
using Hospital_Management_System.Models.EmailSettings;
using Hospital_Management_System.Services;
using Hospital_Management_System.Services.Interface;
using Microsoft.AspNetCore.Connections;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Register MVC Controllers
builder.Services.AddControllers();

// Register services required for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Dependency Injection

// Database connection factory
builder.Services.AddScoped<SqlConnectionFactory>();

// Patient module dependencies
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientServices, PatientServices>();

// Doctor module dependencies
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

// Appointment module dependencies
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Report module dependencies
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

// EmailService
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

// Build the application
var app = builder.Build();


//
// Middleware Pipeline
// 

// Enable Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authorization middleware
app.UseAuthorization();

// Global exception handling middleware
// Catches unhandled exceptions and returns a standardized response
app.UseMiddleware<GlobalExceptionMiddleware>();

// Logs incoming requests and response details
app.UseMiddleware<RequestLoggingMiddleware>();

// Map controller endpoints
app.MapControllers();

// Start the application
app.Run();


// Project Flow:
// Client Request
//      
// RequestLoggingMiddleware
//      
// GlobalExceptionMiddleware
//      
// Controller
//      
// Service Layer
// 
// Repository Layer
//      
// Database
//      
// Response returned to Client