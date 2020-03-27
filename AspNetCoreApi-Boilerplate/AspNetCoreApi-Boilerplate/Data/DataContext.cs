using AspNetCoreApi_Boilerplate.Data.Entities;
using AspNetCoreApi_Boilerplate.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace AspNetCoreApi_Boilerplate.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ApplyConfigurations(builder);
            SeedData(builder);
        }

        public virtual void ApplyConfigurations(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var applyGenericMethod = typeof(ModelBuilder)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "ApplyConfiguration"
                    && m.GetParameters()
                        .First()
                        .ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                        .GetGenericTypeDefinition());

            var types = Assembly.GetCallingAssembly().GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters);

            foreach (var type in types)
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    if (@interface.IsConstructedGenericType && @interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(builder, new object[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
        }

        public void SeedData(ModelBuilder builder)
        {
            var adminPasswordHasher = new PasswordHash("P@ssword123");

            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "Seeded",
                LastName = "Admin",
                Email = "admin@admin.com",
                PasswordHash = adminPasswordHasher.Hash,
                PasswordSalt = adminPasswordHasher.Salt
            });

            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin"
            });

            builder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 1,
                UserId = 1,
                RoleId = 1
            });
        }

    }
}
