/// <summary>
/// Response da listagem de colaboradores.
/// </summary>
public class GetListDepartmentResponse
{
    /// <summary>
    /// Lista de colaboradores encontrados.
    /// </summary>
    public List<GetListDepartmentDto> Departments { get; set; } = new();

    /// <summary>
    /// Total de registros encontrados.
    /// </summary>
    public int TotalCount { get; set; }
}

/// <summary>
/// DTO de saída para listagem de departamentos.
/// </summary>
public class GetListDepartmentDto
{
    /// <summary>
    /// Identificador único do departamento.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do departamento.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Nome do gestor responsável pelo departamento (opcional).
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// Nome do departamento pai na hierarquia (opcional).
    /// </summary>
    public string? ParentDepartmentName { get; set; }
}