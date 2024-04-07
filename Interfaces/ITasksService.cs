using Task = myTasks.Models.Task;
namespace myTasks.Interfaces;

public interface ITasksService
{
    List<Task> GetAll(int user_id);

    Task GetById(int id);
    
    int Add(Task newTask);
 
    bool Update(int id, Task newTask);
    
    bool Delete(int id);

    void DeleteByUserId(int id);

}