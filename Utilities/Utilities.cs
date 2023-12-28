using Microsoft.Extensions.DependencyInjection;
using myTasks.Interfaces;

namespace Utilities{
    using myTasks.Services;
    public static  class Utilities{
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITasksService,TasksService>();
        }
    }
    

}
