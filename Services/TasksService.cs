using myTasks.Models;
using Task = myTasks.Models.Task;
namespace myTasks.Services;

public static class TasksService
{
    private static List<Task> tasks;

    static TasksService()
    {
        tasks = new List<Task>
        {
            new Task { Id = 1, Name = "Java", Description = "to listen to lesson 14", IfDone=false,DateCreate=new DateTime(2023,12,19),DedlineInDays=5},
            new Task { Id = 2, Name = "OOP", Description = "to learn to the test", IfDone=false,DateCreate=new DateTime(2023,12,19),DedlineInDays=30}
        };
    }

    public static List<Task>GetAll() => tasks;

    public static Task GetById(int id) 
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public static int Add(Task newTask)
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
  
    public static bool Update(int id, Task newTask)
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

      
    public static bool Delete(int id)
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