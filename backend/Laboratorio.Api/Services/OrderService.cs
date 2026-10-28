using Dapper;
using Laboratorio.Api.Exceptions;
using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Models;
using Microsoft.Data.SqlClient;

namespace Laboratorio.Api.Services;

public class OrderService : IOrderService
{
    private readonly string _connectionString;

    public OrderService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LaboratorioDb")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'LaboratorioDb'.");
    }

    public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto order)
    {
        if (order.IdPatient <= 0)
            throw new BadRequestException("El paciente es obligatorio.");

        if (order.DateAtention == default)
            throw new BadRequestException("La fecha de atención es obligatoria.");

        if (order.DateAtention.Date > DateTime.Today)
            throw new BadRequestException("La fecha de atención no puede ser futura.");

        if (order.IdExams is null || order.IdExams.Count == 0)
            throw new BadRequestException("Debe seleccionar al menos un examen.");

        if (order.IdExams.Distinct().Count() != order.IdExams.Count)
            throw new BadRequestException("No se puede repetir un examen en la misma orden.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var patientExists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Patient WHERE IdPatient = @IdPatient",
            new { order.IdPatient }, transaction);
        if (patientExists == 0)
            throw new NotFoundException("El paciente no existe.");

        var examsFound = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Exam WHERE IdExam IN @IdExams",
            new { order.IdExams }, transaction);
        if (examsFound != order.IdExams.Count)
            throw new NotFoundException("Alguno de los exámenes seleccionados no existe.");

        var idOrder = await connection.QuerySingleAsync<int>(
            "INSERT INTO Orders (IdPatient, DateOrder) VALUES (@IdPatient, @DateOrder); SELECT CAST(SCOPE_IDENTITY() AS int);",
            new { order.IdPatient, DateOrder = order.DateAtention.Date }, transaction);

        await connection.ExecuteAsync(
            "INSERT INTO OrdersExam (IdOrder, IdExam) VALUES (@IdOrder, @IdExam);",
            order.IdExams.Select(idExam => new { IdOrder = idOrder, IdExam = idExam }), transaction);

        await transaction.CommitAsync();

        return new CreateOrderResponseDto { IdOrder = idOrder };
    }

    private class OrderRow
    {
        public int IdOrder { get; set; }
        public string NamePatient { get; set; } = "";
        public DateTime DateAtention { get; set; }
        public int? IdExam { get; set; }
        public string? CodeExam { get; set; }
        public string? NameExam { get; set; }
    }

    public async Task<List<ItemListOrderDto>> ListOrderAsync()
    {
        const string sql = @"
            SELECT O.IdOrder, P.NamePatient, O.DateOrder AS DateAtention, E.IdExam, E.CodeExam, E.NameExam
            FROM Orders O
            INNER JOIN Patient P ON P.IdPatient = O.IdPatient
            LEFT JOIN OrdersExam OE ON OE.IdOrder = O.IdOrder
            LEFT JOIN Exam E ON E.IdExam = OE.IdExam
            ORDER BY O.IdOrder DESC, E.CodeExam";

        await using var connection = new SqlConnection(_connectionString);
        var rows = await connection.QueryAsync<OrderRow>(sql);

        return rows
            .GroupBy(r => new { r.IdOrder, r.NamePatient, r.DateAtention })
            .Select(g => new ItemListOrderDto
            {
                IdOrder = g.Key.IdOrder,
                NamePatient = g.Key.NamePatient,
                DateAtention = g.Key.DateAtention,
                Exams = g.Where(r => r.IdExam.HasValue)
                         .Select(r => new ExamDto { IdExam = r.IdExam!.Value, CodeExam = r.CodeExam!, NameExam = r.NameExam! })
                         .ToList()
            })
            .ToList();
    }

    public async Task<List<PatientDto>> ListPatientAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        var patients = await connection.QueryAsync<PatientDto>(
            "SELECT IdPatient, NamePatient FROM Patient ORDER BY NamePatient");
        return patients.ToList();
    }

    public async Task<List<ExamDto>> ListExamAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        var exams = await connection.QueryAsync<ExamDto>(
            "SELECT IdExam, CodeExam, NameExam FROM Exam ORDER BY NameExam");
        return exams.ToList();
    }
}