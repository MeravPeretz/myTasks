using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using myTasks.Interfaces;
using myTasks.Models;
using myTasks.Services;
using Microsoft.AspNetCore.Authorization;

namespace mytasks.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
     IUserService UserService;
     ITokenService TokenService;
     ITasksService TasksService;
    public UserController(IUserService UserService,ITokenService TokenService,ITasksService TasksService)
    {
        this.UserService = UserService;
        this.TokenService=TokenService;
        this.TasksService=TasksService;
    }
    [HttpPost("/Login")]
    public ActionResult<String> Login([FromBody] User User){
        try{
            User currentUser =this.UserService.GetAll().Find(u=>u.Password==User.Password&&u.Name==User.Name);
            var claims = new List<Claim>
            {
                new Claim("type", "USER"),
                new Claim("id",(currentUser.Id).ToString()),
            };
            if (currentUser.UserType == UserType.ADMIN)
                claims.Add(new Claim("type","ADMIN"));
            
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        catch{
            return Unauthorized();
        }
    }
    [HttpGet()]
    [Authorize(Policy = "ADMIN")]
    public ActionResult<List<User>> Get()
    {
        return UserService.GetAll();
    }

    [HttpGet("Current")]
    [Authorize(Policy = "USER")]
    public ActionResult<User> GetCurrent()
    {
        int user_id=int.Parse(User.FindFirst("id").Value);
        System.Console.WriteLine(user_id);
        var user = UserService.GetById(user_id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "ADMIN")]

    public ActionResult Post(User newUser)
    {
        var newId = UserService.Add(newUser);
        return CreatedAtAction("Post", 
            new {id = newId}, UserService.GetById(newId));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ADMIN")]
    public ActionResult Put(int id,User newUser)
    {
        var result = UserService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ADMIN")]
        public ActionResult Delete(int id)
        {
            var result = UserService.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            TasksService.DeleteByUserId(id);
            return NoContent();
    } 
}

