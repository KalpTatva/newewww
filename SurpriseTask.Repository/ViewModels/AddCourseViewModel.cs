using System.ComponentModel.DataAnnotations;

namespace SurpriseTask.Repository.ViewModels;

public class AddCourseViewModel
{
    [Required(ErrorMessage = "Course name is required")]
    public string? CourseName {get;set;}

    [Required(ErrorMessage = "Course content is required")]
    public string? CourseContent {get;set;}

    [Required(ErrorMessage = "Credits are required")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Credits input.")]
    public int Credits {get;set;}

    [Required(ErrorMessage = "Department is required")]
    public string? Department {get;set;}

    public int UserId {get;set;}
    public int CourseId {get;set;}
}
