using Microsoft.AspNetCore.Mvc;
using myTasks.Interfaces;
using myTasks.Models;

namespace mytasks.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    ILoginService LoginService;
    public LoginController(ILoginService LoginService)
    {
        this.LoginService = LoginService;
    }
    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return LoginService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var User = LoginService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = LoginService.Add(newUser);

        return CreatedAtAction("Post", 
            new {id = newId}, LoginService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,User newUser)
    {
        var result = LoginService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
        public ActionResult Delete(int id)
    {
        var result = LoginService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    } 
}
