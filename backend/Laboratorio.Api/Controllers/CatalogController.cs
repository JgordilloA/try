using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio.Api.Controllers;

[ApiController]
[Route("api")]
public class CatalogController(IOrderService orderService) : ControllerBase
{
    // GET /api/patients
    [HttpGet("patients")]
    public async Task<ActionResult<List<PatientDto>>> Patients() =>
        Ok(await orderService.ListPatientAsync());

    // GET /api/exams
    [HttpGet("exams")]
    public async Task<ActionResult<List<ExamDto>>> Exams() =>
        Ok(await orderService.ListExamAsync());
}