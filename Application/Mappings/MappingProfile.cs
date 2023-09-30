using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Mappings
{
    public class MappingProfile : BaseMappingProfile
    {
        public MappingProfile(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void AddProfiles()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly(), _serviceProvider);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly, IServiceProvider serviceProvider)
        {
            Type mapFromType = typeof(IMapFrom<>);
            string mappingMethodName = nameof(IMapFrom<object>.Mapping);
            bool HasInterface(Type t)
            {
                return t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
            }

            List<Type> types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
            Type[] argumentTypes = new Type[] { typeof(Profile) };
            foreach (Type? type in types)
            {
                object instance = GetInstanceOf(type, serviceProvider);

                MethodInfo? methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    List<Type> interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count > 0)
                    {
                        foreach (Type? @interface in interfaces)
                        {
                            MethodInfo? interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                            interfaceMethodInfo?.Invoke(instance, new object[] { this });
                        }
                    }
                }
            }
        }

        private static object GetInstanceOf(Type type, IServiceProvider serviceProvider)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(type)!;
            }

            // Type without parameterless constructor
            return serviceProvider.GetRequiredService(type);
        }
    }
}
