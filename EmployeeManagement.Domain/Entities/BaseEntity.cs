namespace EmployeeManagement.Domain.Entities
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador da entidade.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
