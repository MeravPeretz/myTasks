using User = myTasks.Models.User;
namespace myTasks.Interfaces;

public interface ILoginService
{
    List<User> GetAll();

    User GetById(int id);
    
    int Add(User newUser);
 
    bool Update(int id, User newUser);
    
    bool Delete(int id);
}