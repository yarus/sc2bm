using System;
using System.Web.Http;
using Microsoft.Practices.Unity;
using SC2BM.BusinessFacade.Unity;
using Unity;
using Unity.WebApi;

namespace SC2BM.WebSite.Classes.Unity
{
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<Microsoft.Practices.Unity.IUnityContainer> container = new Lazy<Microsoft.Practices.Unity.IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static Microsoft.Practices.Unity.IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return container.Value.Resolve<T>();
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            return container.Value.Resolve(type);
        }

        #endregion

        private static void RegisterTypes(Microsoft.Practices.Unity.IUnityContainer container)
        {
            UnityServiceBootstrapper.RegisterTypes(container);

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
        }
    }
}
