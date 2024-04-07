using Microsoft.Extensions.DependencyInjection;
using myTasks.Interfaces;

namespace myTasks.Utilities{
    using myTasks.Services;
    public static  class Utilities{
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITasksService,TasksService>();
        }
         public static void AddUser(this IServiceCollection services)
        {
            services.AddSingleton<IUserService,UserService>();
        }
        public static void AddToken(this IServiceCollection services)
        {
            services.AddSingleton<ITokenService,TokenService>();
        }
        
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITokenService,TokenService>();
            services.AddSingleton<IUserService,UserService>();
            services.AddSingleton<ITasksService,TasksService>();
        }
    }
}
