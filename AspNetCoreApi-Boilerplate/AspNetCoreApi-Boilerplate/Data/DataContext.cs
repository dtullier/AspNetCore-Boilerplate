using AspNetCoreApi_Boilerplate.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace AspNetCoreApi_Boilerplate.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ApplyConfigurations(builder);
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

    }
}
