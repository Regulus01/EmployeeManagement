using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.Inputs
{
    public class CreateDepartmentInputModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Gerente")]
        public Guid? ManagerId { get; set; }

        [Display(Name = "Departamento Pai")]
        public Guid? ParentDepartmentId { get; set; }
    }
}
