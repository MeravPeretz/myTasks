using User = myTasks.Models.User;
using myTasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace myTasks.Services;
public class LoginService: ILoginService
{
    private  List<User> users;
    private string fileName = "Users.json";

    public LoginService()
    {
        this.fileName = Path.Combine( "Data", "users.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(users));
    }
    public  List<User> GetAll(){return users;}

    public  User GetById(int id) 
    {
        return users.FirstOrDefault(t => t.Id == id);
    }

    public  int Add(User newUser)
    {
        if (users.Count == 0)
            newUser.Id = 1;
        else
            newUser.Id =  users.Max(t => t.Id) + 1;
        users.Add(newUser);
        saveToFile();
        return newUser.Id;
    }
  
    public  bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser= GetById(id);
        if (existingUser== null )
            return false;

        var index = users.IndexOf(existingUser);
        if (index == -1 )
            return false;

        users[index] = newUser;
        saveToFile();
        return true;
    }  

      
    public  bool Delete(int id)
    {
        var existingUser= GetById(id);
        if (existingUser== null )
            return false;

        var index = users.IndexOf(existingUser);
        if (index == -1 )
            return false;

        users.RemoveAt(index);
        saveToFile();
        return true;
    }  
    public int Count => users.Count();
}
