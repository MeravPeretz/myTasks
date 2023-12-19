namespace myTasks.Models;
public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IfDone { get; set; }
    public DateTime DateCreate { get; set; }
    public int DedlineInDays { get; set; }
}