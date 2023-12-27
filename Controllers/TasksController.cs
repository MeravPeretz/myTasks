using Microsoft.AspNetCore.Mvc;
using myTasks.Interfaces;
using Task = myTasks.Models.Task;

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
    public ActionResult<List<Task>> Get()
    {
        return TasksService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        var Task = TasksService.GetById(id);
        if (Task == null)
            return NotFound();
        return Task;
    }

    [HttpPost]
    public ActionResult Post(Task newTask)
    {
        var newId = TasksService.Add(newTask);

        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,Task newTask)
    {
        var result = TasksService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
    [HttpDelete]
        public ActionResult Delete(int id)
    {
        var result = TasksService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    } 
}
