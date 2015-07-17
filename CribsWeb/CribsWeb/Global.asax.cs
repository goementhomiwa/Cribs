using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Cribs.Web.Controllers;
using Cribs.Web.Repositories;
using ServiceStack.Redis;

namespace Cribs.Web
{
    public class MvcApplication : HttpApplication
    {
        public IRedisClientsManager ClientsManager;
        private const string RedisUri = "localhost";

        protected void Application_Start()
        {
            ClientsManager = new PooledRedisClientManager(RedisUri);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());

            builder.RegisterModelBinderProvider();

            builder.RegisterType<ChatGroupRepository>()
                .As<IChatGroupRepository>();

            builder.RegisterType<MessageRepository>()
                .As<IMessageRepository>();

            builder.Register(c => ClientsManager.GetClient());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //ClientsManager.Dispose();
            Server.ClearError();
           Response.Redirect("~/Error/Index");
        }

    }
}
