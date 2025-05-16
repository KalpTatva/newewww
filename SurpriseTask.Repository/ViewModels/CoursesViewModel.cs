using SurpriseTask.Repository.Models;

namespace SurpriseTask.Repository.ViewModels;

public class CoursesViewModel
{
    public List<Course>? Courses {get;set;}
    public int TotalItems {get;set;}
    public string? Role {get;set;}
}
