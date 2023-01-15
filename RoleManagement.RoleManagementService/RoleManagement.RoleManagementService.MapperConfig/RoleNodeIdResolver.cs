using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagement.RoleManagementService.MapperConfig
{
    public sealed class RoleNodeIdResolver : IMappingAction<Dto.Role, Role>
    {
        private readonly RoleDbContext dbContext;

        public RoleNodeIdResolver(RoleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Process(Dto.Role source, Role destination, ResolutionContext context)
        {
            
        }
    }
}
