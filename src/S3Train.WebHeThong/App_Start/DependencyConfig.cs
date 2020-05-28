using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Service;
using S3Train.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;

namespace S3Train.WebHeThong.App_Start
{
    public class DependencyConfig
    {
        public static IContainer RegisterDependencyResolvers()
        {
            ContainerBuilder builder = new ContainerBuilder();
            RegisterContext(builder);
            RegisterDependencyMappingDefaults(builder);
            RegisterDependencyMappingOverrides(builder);
            IContainer container = builder.Build();
            // Set Up MVC Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Set Up WebAPI Resolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        private static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacWebTypesModule());

            builder.Register(ctx =>
            {
                HttpContextBase httpContext = ctx.Resolve<HttpContextBase>();
                if (httpContext != null)
                {
                    return httpContext.GetOwinContext();
                }
                return HttpContext.Current.GetOwinContext();
            }).As<IOwinContext>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterDependencyMappingDefaults(ContainerBuilder builder)
        {
            Assembly coreAssembly = Assembly.GetAssembly(typeof(IStateManager));
            Assembly webAssembly = Assembly.GetAssembly(typeof(MvcApplication));

            builder.RegisterAssemblyTypes(coreAssembly).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterControllers(webAssembly); // mvc
            builder.RegisterApiControllers(webAssembly); // web api
            builder.RegisterModule(new AutofacWebTypesModule());
        }

        private static void RegisterDependencyMappingOverrides(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>();
            builder.RegisterType<ChiTietMuonTraService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HopService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HoSoService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<KeService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LoaiHoSoService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MuonTraService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<NoiBanHanhService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PhongBanService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TaiLieuVanBanService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TuService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LichSuHoatDongService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FunctionLichSuHoatDongService>().As<IFunctionLichSuHoatDongService>();
            builder.RegisterType<AccountManager>().As<IAccountManager>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserIdentityService>().As<IUserIdentityService>();
        }
    }
}