using Microsoft.Extensions.DependencyInjection;
using myTasks.Interfaces;

namespace Utilities{
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
    }
    

}
