using Task = myTasks.Models.Task;
namespace myTasks.Interfaces;

public interface ITasksService
{
    List<Task> GetAll();

    Task GetById(int id);
    
    int Add(Task newTask);
 
    bool Update(int id, Task newTask);
    
    bool Delete(int id);
}