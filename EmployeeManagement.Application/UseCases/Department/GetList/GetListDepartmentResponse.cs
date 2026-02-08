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
/// 
public class GetListDepartmentDto
{

    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? ManagerName { get; set; }
    public string? ParentDepartmentName { get; set; }
}