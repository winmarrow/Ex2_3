using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Ex3.DI
{
    public class DiContainer
    {
        private static DiContainer _instance = null;

        /// <summary>
        /// Static instance of container
        /// </summary>
        public static DiContainer Instance => _instance ?? (_instance = new DiContainer());

        private readonly IDictionary<Type, Type> _typesDictionary;

        private readonly IDictionary<Type, object> _instancesDictionary;

        protected DiContainer()
        {
            _typesDictionary = new Dictionary<Type, Type>();
            _instancesDictionary = new Dictionary<Type, object>();
        }

        /// <summary>
        /// This method registers type in the container
        /// </summary>
        /// <typeparam name="TImplementation">Class</typeparam>
        public void Register<TImplementation>()
            where TImplementation : class
        {
            _typesDictionary[typeof(TImplementation)] = typeof(TImplementation);
        }

        /// <summary>
        /// This method registers type in the container for interface
        /// </summary>
        /// <typeparam name="TInterface">Interface</typeparam>
        /// <typeparam name="TImplementation">Class witch implements TInterface</typeparam>
        public void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface
        {
            _typesDictionary[typeof(TInterface)]= typeof(TImplementation);
        }

        /// <summary>
        /// This method registers singleton instance in the container
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="instance"></param>
        public void Register<TInterface>(TInterface instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance),"Instance cannot be null.");
            }
            _instancesDictionary[typeof(TInterface)]= instance;
        }


        /// <summary>
        /// If type T has been registered in the container,
        /// this method returns singleton instance for type T or create new instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        private object GetInstance(Type contract)
        {
            if (_instancesDictionary.ContainsKey(contract))
            {
                return _instancesDictionary[contract];
            }

            if (_typesDictionary.ContainsKey(contract))
            {
                Type implementation = _typesDictionary[contract];
                ConstructorInfo[] constructors = GetPublicConstructors(implementation);
                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] constructorParameters = constructor.GetParameters();
                    if (constructorParameters.Length == 0)
                    {
                        return Activator.CreateInstance(implementation);
                    }

                    List<object> parameters = new List<object>(constructorParameters.Length);
                    foreach (ParameterInfo parameterInfo in constructorParameters)
                    {
                        parameters.Add(GetInstance(parameterInfo.ParameterType));
                    }
                    return constructor.Invoke(parameters.ToArray());
                }
            }
            else
            {
                throw new ArgumentException($"Type \"{contract.FullName}\" wasn't registered in container.");
            }

            return null;
        }

        private ConstructorInfo[] GetPublicConstructors(Type type)
        {
            return type.GetConstructors()
                .Where(info => info.IsPublic)
                .ToArray();
        }


    }
}
