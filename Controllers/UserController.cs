using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using myTasks.Interfaces;
using myTasks.Models;

namespace mytasks.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
     IUserService UserService;
     ITokenService TokenService;
    public UserController(IUserService UserService,ITokenService TokenService)
    {
        this.UserService = UserService;
        this.TokenService=TokenService;
    }
    [HttpPost]
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
    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return UserService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var User = UserService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    // [HttpPost]
    // public ActionResult Post(User newUser)
    // {
    //     var newId = UserService.Add(newUser);

    //     return CreatedAtAction("Post", 
    //         new {id = newId}, UserService.GetById(newId));
    // }

    [HttpPut("{id}")]
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
        public ActionResult Delete(int id)
    {
        var result = UserService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    } 
}

