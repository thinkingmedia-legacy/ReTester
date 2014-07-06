using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester.Annotations;

namespace ReTester
{
    public class Maker
    {
        /// <summary>
        /// Containers for factory functions.
        /// </summary>
        private readonly Dictionary<Type, object> _container;

        private readonly Dictionary<string, object> _named; 

        /// <summary>
        /// Constructor
        /// </summary>
        public Maker()
        {
            _container = new Dictionary<Type, object>();
            _named = new Dictionary<string, object>();
        }

        /// <summary>
        /// Adds a factory function for a type to the maker.
        /// </summary>
        /// <typeparam name="TType">The class type</typeparam>
        /// <param name="pFunc">The callback to create the type</param>
        public void HowToMake<TType>([NotNull] Func<TType> pFunc)
            where TType : class
        {
            if (pFunc == null)
            {
                throw new ArgumentNullException("pFunc");
            }

            if (_container.ContainsKey(typeof(TType)))
            {
                throw new ArgumentException("Type already known by maker.");
            }

            _container.Add(typeof(TType), pFunc);
        }

        /// <summary>
        /// Adds a factory function for a type and assigns it to a name.
        /// </summary>
        /// <typeparam name="TType">The class type</typeparam>
        /// <param name="pName">The unique name</param>
        /// <param name="pFunc">The callback to create the type</param>
        public void HowToMake<TType>([NotNull] string pName, [NotNull] Func<TType> pFunc)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }
            if (pFunc == null)
            {
                throw new ArgumentNullException("pFunc");
            }
            if (_named.ContainsKey(pName))
            {
                throw new ArgumentException(string.Format("Name [{0}] is already used.", pName));
            }

            _named.Add(pName, pFunc);
        }

        /// <summary>
        /// Adds an object instance to the maker for a given type.
        /// </summary>
        /// <typeparam name="TType">The class type</typeparam>
        /// <param name="pObject">The object instance to add</param>
        public void UseAs<TType>([NotNull] TType pObject)
            where TType : class
        {
            if (pObject == null)
            {
                throw new ArgumentNullException("pObject");
            }

            if (_container.ContainsKey(typeof (TType)))
            {
                throw new ArgumentException("Type already known by maker.");
            }

            Func<TType> func = ()=>pObject;
            _container.Add(typeof(TType), func);
        }

        /// <summary>
        /// Create a new instance of a type.
        /// </summary>
        /// <typeparam name="TType">The type</typeparam>
        /// <param name="pName">The unique name</param>
        /// <returns>The new instance</returns>
        public TType Make<TType>([NotNull] string pName)
            where TType : class
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            Assert.IsTrue(_named.ContainsKey(pName),
                string.Format("Name is not known [{0}]", pName));

            Type type = typeof(TType);
            Func<TType> pFunc = (Func<TType>)_named[pName];

            object value = pFunc();
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, type);

            return (TType)value;
        }

        /// <summary>
        /// Create a new instance of a type.
        /// </summary>
        /// <typeparam name="TType">The type</typeparam>
        /// <returns>The new instance</returns>
        public TType Make<TType>()
            where TType : class
        {
            Type type = typeof (TType);
            Assert.IsTrue(_container.ContainsKey(type),
                string.Format("Container does not know how to make [{0}]", type.Name));

            Func<TType> pFunc = (Func<TType>)_container[type];

            object value = pFunc();
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, type);

            return (TType)value;
        }
    }
}