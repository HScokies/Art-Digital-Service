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
                            Roles.Permissions.createUsers,
                            Roles.Permissions.createStaff,
                            Roles.Permissions.createCases,
                            Roles.Permissions.readUsers,
                            Roles.Permissions.readStaff,
                            Roles.Permissions.readCases,
                            Roles.Permissions.updateUsers,
                            Roles.Permissions.updateStaff,
                            Roles.Permissions.updateCases,
                            Roles.Permissions.deleteUsers,
                            Roles.Permissions.deleteStaff,
                            Roles.Permissions.deleteCases,
                            Roles.Permissions.utilsAccess
                        } 
                    },
                    new StaffRoleDto()
                    {
                        id = 2,
                        name = "Модератор",
                        PermissionsList =
                        {
                            Roles.Permissions.createUsers,
                            Roles.Permissions.readUsers,
                            Roles.Permissions.updateUsers,
                            Roles.Permissions.deleteUsers
                        }
                    },
                    new StaffRoleDto()
                    {
                        id = 3,
                        name = "Преподаватель",
                        PermissionsList =
                        {
                            Roles.Permissions.readUsers,
                            Roles.Permissions.rateUsers,
                        }
                    }
                ); 
            
        }
    }
}
