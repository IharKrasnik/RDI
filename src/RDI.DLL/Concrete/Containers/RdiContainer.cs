using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RDI.Attributes;
using RDI.Interfaces.Containers;

namespace RDI.Concrete.Containers
{
    public class RdiContainer : IGeneralRdiContainer
    {
        protected List<IRdiSettingsContainer> settingsContainers = new List<IRdiSettingsContainer>();

        public IRdiSettingsContainer Resolve<T>()
        {
            Type interfaceType = typeof(T);
            
            var settingsContainer = new RdiSettingsContainer();
            settingsContainer.InterfaceType = interfaceType;
            settingsContainers.Add(settingsContainer);
            return settingsContainer;
        }
      
        public T Get<T>(string injectName = null)
        {
            Type interfaceType = typeof(T);
            var settingsContainer = GetSettingsContainer(interfaceType, injectName);
            return CreateType<T>(settingsContainer);
        }

        private IRdiSettingsContainer GetSettingsContainer(Type interfaceType, string name)
        {
            var typeSettingsContainers = settingsContainers.Where((f) => f.InterfaceType.Equals(interfaceType));
            if (typeSettingsContainers.Count() == 0)
                return null;
            if (typeSettingsContainers.Count() == 1)
                return typeSettingsContainers.First();
            if (String.IsNullOrEmpty(name))
                throw new InvalidOperationException("There are multiple resolve types for " + interfaceType.ToString());
            var result = typeSettingsContainers.FirstOrDefault(f => String.Equals(f.BindingSettings.InjectName, name));
            if (result != null)
                return result;
            result = new RdiSettingsContainer(new RdiBindingSettings().WhenName(name))
            {
                InterfaceType = interfaceType               
            };
            return result;
        }
     
        /// <summary>
        /// Constructing object from inject setiings
        /// </summary>
        /// <typeparam name="T">interfaceType</typeparam>
        /// <param name="settingsContainer">current settings</param>
        /// <returns></returns>
        private T CreateType<T>(IRdiSettingsContainer settingsContainer)
        {
            Type resolveType = settingsContainer!=null ? settingsContainer.ResolvedType ?? settingsContainer.InterfaceType
                                                        :typeof(T) ;
            var ctor = GetInjectConstructor(resolveType);
            if (ctor != null)
            {
                var ctorParamsValues = ctor.GetParameters().Select(p =>
                {
                    if (settingsContainer != null)
                        if (settingsContainer.BindingSettings != null)
                        {
                            if (settingsContainer.BindingSettings.ConstructorParameters.Keys.Contains(p.Name))
                                return settingsContainer.BindingSettings.ConstructorParameters[p.Name];
                        }

                    if (p.IsOptional && p.HasDefaultValue)
                        return p.DefaultValue;
                    var injectAttr = p.GetCustomAttribute(typeof(RDInjectAttribute)) as RDInjectAttribute;
                    string  injectName =injectAttr!=null ? injectAttr.Name : null;
                    
                    return CallGetMethod(p.ParameterType, injectName);
                }).ToArray();
                return SetInjectionProperties((T)Activator.CreateInstance(resolveType, ctorParamsValues)); ;
            }
            if (settingsContainer != null && settingsContainer.ResolvedType!=null)
            {
                return (T)CallGetMethod(settingsContainer.ResolvedType);
            }
            if (resolveType.IsInterface)
                throw new Exception("Cannot create instance of interface " + resolveType.ToString());            
            return SetInjectionProperties((T)Activator.CreateInstance(resolveType));
        }

        /// <summary>
        /// Calling Get<> method with specifying type parameter
        /// </summary>
        /// <param name="type">type parameter for get<> method </param>
        /// <returns>value of evaluated get<> method</returns>
        protected object CallGetMethod(Type type, string injectName = null)
        {
            MethodInfo getMethod = typeof(IGeneralRdiContainer).GetMethod("Get");
            var getMethodGeneric = getMethod.MakeGenericMethod(type);
            return getMethodGeneric.Invoke(this, new[] { injectName });
        }

        /// <summary>
        /// Set values of properties with RDInject Attribute
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="obj">constructed before object</param>
        /// <returns>same object with properties set</returns>
        protected virtual T SetInjectionProperties<T>(T obj)
        {
            var type = obj.GetType();
            var injProperties = type.GetProperties().Where(p => p.GetCustomAttribute(typeof(RDInjectAttribute)) != null);
            foreach (var prop in injProperties)
            {
                var injectAttr = prop.GetCustomAttribute(typeof(RDInjectAttribute)) as RDInjectAttribute;
                string injectName = injectAttr != null ? injectAttr.Name : null;

                var value = CallGetMethod(prop.PropertyType, injectName);
                prop.SetValue(obj, value);
            }
            return obj;
        }

        /// <summary>
        /// Get suitable constructor from type. Priority (From High to Low) : RDInjectAttribute - Default Constructor - First of all constructors
        /// </summary>
        /// <param name="type">type where constructors are searched</param>
        /// <returns>found constructor or null</returns>
        protected  virtual ConstructorInfo GetInjectConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
                return null;

            var ctor = constructors.FirstOrDefault(c => c.GetCustomAttribute(typeof(RDInjectAttribute)) != null);
            if (ctor != null)
                return ctor;

            return type.GetConstructor(Type.EmptyTypes) ?? constructors[0];
        }
    }
}
