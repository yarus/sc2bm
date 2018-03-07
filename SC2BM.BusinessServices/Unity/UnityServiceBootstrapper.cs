using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using SC2BM.BusinessFacade.Routine;
using SC2BM.BusinessFacade.Services;
using SC2BM.DataAccess.Repositories;
using SC2BM.ServiceModel.Repositories;

namespace SC2BM.BusinessFacade.Unity
{
	public static class UnityServiceBootstrapper
	{
		public static void RegisterTypes(IUnityContainer container)
		{
		    if (container == null)
		    {
                throw new ArgumentNullException("Unity container can not be NULL");   
		    }

			container.AddNewExtension<Interception>();

			// Register services with interceptor
			container.RegisterTypes(new List<Type>
			{
                typeof(LogService),
                typeof(SmtpService),
				typeof(BuildOrderService),
                typeof(CommentService),
                typeof(UserService),
                typeof(AuthorizationService),
                typeof(NewsService),
                typeof(RateService),
                typeof(LinkService),
                typeof(BuildProcessorService),
                typeof(BlogService),
                typeof(BlogPostService)
			},
				WithMappings.FromMatchingInterface,
				getInjectionMembers: t => new InjectionMember[]
				{
					new Interceptor<InterfaceInterceptor>(),
					new InterceptionBehavior<ExceptionHandlingInterceptorBehavior>()
				});

			container.RegisterType<IBuildOrderRepository, BuildOrderRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<INewsRepository, NewsRepository>();
		    container.RegisterType<ILogRepository, LogRepository>();
            container.RegisterType<IRateRepository, RateRepository>();
            container.RegisterType<ILinkRepository, LinkRepository>();
            container.RegisterType<IBlogsRepository, BlogsRepository>();
            container.RegisterType<IBlogPostRepository, BlogPostRepository>();
		}
	}
}