namespace Laboratorio.Api.Models;

public class CreateOrderDto
{
    public int IdPatient { get; set; }
    public DateTime DateAtention { get; set; }
    public List<int> IdExams { get; set; } = [];
}

public class CreateOrderResponseDto
{
    public int IdOrder { get; set; }
}

public class ExamDto
{
    public int IdExam { get; set; }
    public string CodeExam { get; set; } = "";
    public string NameExam { get; set; } = "";
}

public class ItemListOrderDto
{
    public int IdOrder { get; set; }
    public string NamePatient { get; set; } = "";
    public DateTime DateAtention { get; set; }
    public List<ExamDto> Exams { get; set; } = [];
}

public class PatientDto
{
    public int IdPatient { get; set; }
    public string NamePatient { get; set; } = "";
}