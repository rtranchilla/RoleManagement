using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagement.RoleManagementService.Validation
{
    public interface IValidator<T> where T : class, IEntity
    {
        bool Validate(T entity);
    }
}
