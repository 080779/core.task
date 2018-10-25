using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Service
{
    public static class EFCoreExtensions
    {
        private static bool IsIEntityTypeConfigurationType(Type typeIntf)
        {
            return typeIntf.IsInterface && typeIntf.IsGenericType && typeIntf.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
        }
        public static void ApplyConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            //判断这个类型实现的接口是不是IEntityTypeConfiguration<>类型，因为是泛型的，所以写的就比较麻烦
            var types = assembly.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces().Any(it => IsIEntityTypeConfigurationType(it)));
            Type typeModelBuilder = modelBuilder.GetType();
            MethodInfo methodNonGenericApplyConfiguration = typeModelBuilder.GetMethods().SingleOrDefault(t=>t.Name== "ApplyConfiguration" && t.GetParameters().Any(p=>p.ParameterType.Name.Contains("IEntityTypeConfiguration")));
            //MethodInfo methodNonGenericApplyConfiguration = typeModelBuilder.GetMethods().Where(t => t.Name == nameof(modelBuilder.ApplyConfiguration)).First();
            foreach (var type in types)
            {
                var entityTypeConfig = Activator.CreateInstance(type);
                //获取实体的类型
                Type typeEntity = type.GetInterfaces().First(t => IsIEntityTypeConfigurationType(t)).GenericTypeArguments[0];
                //因为ApplyConfiguration是泛型方法，所以要通过MakeGenericMethod转换为泛型方法才能调用
                MethodInfo methodApplyConfiguration = methodNonGenericApplyConfiguration.MakeGenericMethod(typeEntity);
                methodApplyConfiguration.Invoke(modelBuilder, new[] { entityTypeConfig });
            }
        }
    }
}
