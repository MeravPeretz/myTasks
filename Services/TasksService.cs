using Task = myTasks.Models.Task;
using myTasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace myTasks.Services;

public class TasksService: ITasksService
{
    private  List<Task> tasks;

    public TasksService()
    {
        tasks = new List<Task>
        {
            new Task { Id = 1, Name = "Java", Description = "to listen to lesson 14", IfDone=false,DateCreate=new DateTime(2023,12,19),DedlineInDays=5},
            new Task { Id = 2, Name = "OOP", Description = "to learn to the test", IfDone=false,DateCreate=new DateTime(2023,12,19),DedlineInDays=30}
        };
    }

    public  List<Task> GetAll(){return tasks;}

    public  Task GetById(int id) 
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public  int Add(Task newTask)
    {
        if (tasks.Count == 0)
        {
            newTask.Id = 1;
        }
        else
        {
            newTask.Id =  tasks.Max(t => t.Id) + 1;
        }

        tasks.Add(newTask);

        return newTask.Id;
    }
  
    public  bool Update(int id, Task newTask)
    {
        if (id != newTask.Id)
            return false;

        var existingTask= GetById(id);
        if (existingTask== null )
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1 )
            return false;

        tasks[index] = newTask;

        return true;
    }  

      
    public  bool Delete(int id)
    {
        var existingTask= GetById(id);
        if (existingTask== null )
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1 )
            return false;

        tasks.RemoveAt(index);
        return true;
    }  



}

public static class TasksUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITasksService, TasksService>();
    }
}