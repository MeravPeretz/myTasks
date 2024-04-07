using Task = myTasks.Models.Task;
using myTasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;



namespace myTasks.Services;
public class TasksService: ITasksService
{
    private  List<Task> tasks;
    private string fileName = "Tasks.json";

    public TasksService()
    {
        this.fileName = Path.Combine( "Data", "tasks.json");

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
    public  List<Task> GetAll(int user_id){
        return new List<Task>(tasks.Where(t=>t.OwnerId==user_id));
    }

    public  Task GetById(int id) 
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public  int Add(Task newTask)
    {
        if (tasks.Count == 0)
            newTask.Id = 1;
        else
            newTask.Id =  tasks.Max(t => t.Id) + 1;
        tasks.Add(newTask);
        saveToFile();
        return newTask.Id;
    }
  
    public  bool Update(int id, Task newTask)
    {
        if (id != newTask.Id){
            System.Console.WriteLine("id error");
            return false;

        }

        var existingTask= GetById(id);
        if (existingTask== null )
            {
            System.Console.WriteLine("task not exist");
            return false;
        }

        var index = tasks.IndexOf(existingTask);
        if (index == -1 )
            {
            System.Console.WriteLine("error");
            return false;

        }

        tasks[index] = newTask;
        saveToFile();
        return true;
    }  

      
    public  bool Delete(int id)
    {
        var existingTask= GetById(id);
        if (existingTask== null )
            return false;

        tasks.Remove(existingTask);
        saveToFile();
        return true;
    }  
    public int Count => tasks.Count();

    public void DeleteByUserId(int id){

        tasks=new List<Task>(tasks.Where(task=>task.OwnerId!=id));
        saveToFile();
    }
}

