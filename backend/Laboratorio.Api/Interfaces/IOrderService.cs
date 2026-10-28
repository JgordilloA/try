using Laboratorio.Api.Models;

namespace Laboratorio.Api.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto order);
    Task<List<ItemListOrderDto>> ListOrderAsync();
    Task<List<PatientDto>> ListPatientAsync();
    Task<List<ExamDto>> ListExamAsync();
}