using Task = myTasks.Models.Task;
using myTasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;


namespace myTasks.Services;
public class TasksService: ITasksService
{
    private  List<Task> tasks;
    private string fileName = "tasks.json";

    public TasksService(IWebHostEnvironment webHost)
    {
        this.fileName = Path.Combine(webHost.ContentRootPath, "data", "tasks.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
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
    public int Count => tasks.Count();
}

public static class TasksUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITasksService, TasksService>();
    }
}