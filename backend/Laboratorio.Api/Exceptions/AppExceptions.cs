namespace Laboratorio.Api.Exceptions;

// Datos de entrada inválidos -> 400
public class BadRequestException(string message) : Exception(message);

// Recurso inexistente -> 404
public class NotFoundException(string message) : Exception(message);