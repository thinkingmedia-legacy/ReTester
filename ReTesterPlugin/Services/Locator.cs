using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ReTesterPlugin.Exceptions;
using ReTesterPlugin.Services.Impl;

namespace ReTesterPlugin.Services
{
    /// <summary>
    /// A very basic service locator.
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static Locator _instance;

        /// <summary>
        /// Storage of all the service objects.
        /// </summary>
        private readonly Dictionary<Type, object> _services;

        /// <summary>
        /// Global access
        /// </summary>
        [NotNull]
        private static Locator Instance
        {
            get { return _instance ?? (_instance = new Locator()); }
        }

        /// <summary>
        /// Adds an object as a service. Will fail is service already exists for that type.
        /// </summary>
        private void Put<T>([NotNull] T pService) where T : class
        {
            if (pService == null)
            {
                throw new ArgumentNullException("pService");
            }

            _services.Add(typeof (T), pService);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Locator()
        {
            _services = new Dictionary<Type, object>();

            Put<iNamingService>(new StandardNamingPattern());
            Put<iAppTheme>(new AppTheme());
        }

        /// <summary>
        /// Gets a service from the container.
        /// </summary>
        [NotNull]
        public static T Get<T>() where T : class
        {
            if (!Instance._services.ContainsKey(typeof (T)))
            {
                throw new ServiceNotFoundException(typeof (T));
            }
            return (T)Instance._services[typeof (T)];
        }
    }
}