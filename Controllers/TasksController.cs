using Microsoft.AspNetCore.Mvc;
using myTasks.Interfaces;
using Task = myTasks.Models.Task;
using Microsoft.AspNetCore.Authorization;


namespace myTasks.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    ITasksService TasksService;

    public TasksController(ITasksService TasksService)
    {
        this.TasksService = TasksService;
    }

    [HttpGet]
    [Authorize(Policy = "USER")]
    public ActionResult<List<Task>> Get()
    {
        return TasksService.GetAll(int.Parse(User.FindFirst("id").Value));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "USER")]
    public ActionResult<Task> Get(int id)
    {
        var Task = TasksService.GetById(id);
        if (Task == null)
            return NotFound();
        if(!CheckAthorization(id))
            return Unauthorized();      
        return Task;
    }

    [HttpPost]
    [Authorize(Policy = "USER")]
    public ActionResult Post(Task newTask)
    {
        newTask.OwnerId=int.Parse(User.FindFirst("id").Value);
        var newId = TasksService.Add(newTask);
        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(newId));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "USER")]
    public ActionResult Put(int id,Task newTask)
    {
        newTask.OwnerId=int.Parse(User.FindFirst("id").Value);
        if(TasksService.GetById(id)==null)
            return NotFound();
        if(!CheckAthorization(id))
            return Unauthorized();
        var result = TasksService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "USER")]
    public ActionResult Delete(int id)
    {
        if(TasksService.GetById(id)==null)
            return NotFound();
        if(!CheckAthorization(id))
            return Unauthorized();
        TasksService.Delete(id);
        return NoContent();
    } 

    private  bool CheckAthorization(int taskId) {
        return TasksService.GetById(taskId).OwnerId==int.Parse(User.FindFirst("id").Value);
    }   
}
