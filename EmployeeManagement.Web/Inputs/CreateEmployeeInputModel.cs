using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.Inputs
{
    public class CreateEmployeeInputModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CPF")]
        public string CPF { get; set; } = string.Empty;

        [Display(Name = "RG")]
        public string? RG { get; set; }

        [Required]
        [Display(Name = "Departamento")]
        public Guid DepartmentId { get; set; }
    }
}
