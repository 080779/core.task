using Microsoft.EntityFrameworkCore;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class EFCoreExtensions
    {
        #region EFCore的config系统自动ApplyConfiguration加载
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
        #endregion

        #region 根据参数表名获取参数值
        /// <summary>
        /// 根据参数表名获取参数值异步扩展方法,返回string类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async static Task<string> GetStringParamAsync(this MyDbContext dbc, string name)
        {
            return await dbc.GetParameterAsync<SettingEntity>(s => s.Name == name, s => s.Parameter);
        }

        /// <summary>
        /// 根据参数表名获取参数值扩展方法,返回string类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetStringParam(this MyDbContext dbc, string name)
        {
            return dbc.GetParameter<SettingEntity>(s => s.Name == name, s => s.Parameter);
        }

        /// <summary>
        /// 根据参数表名获取参数值异步扩展方法,返回decimal类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async static Task<decimal> GetDecimalParamAsync(this MyDbContext dbc, string name)
        {
            decimal param;
            decimal.TryParse(await dbc.GetParameterAsync<SettingEntity>(s => s.Name == name, s => s.Parameter), out param);
            return param;
        }

        /// <summary>
        /// 根据参数表名获取参数值扩展方法,返回decimal类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static decimal GetDecimalParam(this MyDbContext dbc, string name)
        {
            decimal param;
            decimal.TryParse(dbc.GetParameter<SettingEntity>(s => s.Name == name, s => s.Parameter), out param);
            return param;
        }

        /// <summary>
        /// 根据参数表名获取参数值异步扩展方法,返回int类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async static Task<int> GetIntParamAsync(this MyDbContext dbc, string name)
        {
            int param;
            int.TryParse(await dbc.GetParameterAsync<SettingEntity>(s => s.Name == name, s => s.Parameter), out param);
            return param;
        }

        /// <summary>
        /// 根据参数表名获取参数值扩展方法,返回int类型
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIntParam(this MyDbContext dbc, string name)
        {
            int param;
            int.TryParse(dbc.GetParameter<SettingEntity>(s => s.Name == name, s => s.Parameter), out param);
            return param;
        }
        #endregion
    }
}
