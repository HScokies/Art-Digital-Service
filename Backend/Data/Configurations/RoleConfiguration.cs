using Domain.Entities;
using Domain.Enumeration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<StaffRoleDto>
    {
        public void Configure(EntityTypeBuilder<StaffRoleDto> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(r => r.id);

            builder.HasMany(r => r.Staff).WithOne(s => s.Role).HasForeignKey(r => r.roleId);

            builder.HasData(
                    new StaffRoleDto() { 
                        id = 1, 
                        name = "Администратор",
                        PermissionsList = {
                            access_levels.createUsers,
                            access_levels.createStaff,
                            access_levels.createCases,
                            access_levels.readUsers, 
                            access_levels.readStaff, 
                            access_levels.readCases,
                            access_levels.updateUsers,
                            access_levels.updateStaff,
                            access_levels.updateCases,
                            access_levels.deleteUsers,
                            access_levels.deleteStaff,
                            access_levels.deleteCases
                        } 
                    },
                    new StaffRoleDto()
                    {
                        id = 2,
                        name = "Модератор",
                        PermissionsList =
                        {
                            access_levels.createUsers,
                            access_levels.readUsers,
                            access_levels.updateUsers, 
                            access_levels.deleteUsers
                        }
                    },
                    new StaffRoleDto()
                    {
                        id = 3,
                        name = "Преподаватель",
                        PermissionsList =
                        {
                            access_levels.readUsers,
                            access_levels.rateUsers
                        }
                    }
                ); 
            
        }
    }
}
