using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Entities
{
    internal abstract class BaseEntity
    {
        public Guid Id { get; private set; }
    }
}
