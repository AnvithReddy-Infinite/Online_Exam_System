using FinalProject.Controllers;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using FinalProject.Services.Implementations;
using FinalProject.Services.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace OES_WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<OnlineExamSystemEntities>(new HierarchicalLifetimeManager());

            container.RegisterType<IExamRepository, ExamRepository>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IOptionRepository, OptionRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IResultRepository, ResultRepository>();
            container.RegisterType<ITechRepository, TechRepository>();
            container.RegisterType<IExamService, ExamService>();
            container.RegisterType<IUserService, UserService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            try
            {
                container.Resolve<ExamController>();
                container.Resolve<UserController>();
            }
            catch (Exception ex)
            {
                // Log to see exactly which type failed
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}