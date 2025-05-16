namespace SurpriseTask.Repository.ViewModels;

public class EnrolledCourseViewModel
{
    public List<EnrolledIn>? enrolledIns {get;set;}
}

public class EnrolledIn{
    
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public string? CourseName {get;set;}
    public string? CourseContent {get;set;}
    public int Credits {get;set;}
    public string? Department {get;set;}
    public int status {get;set;}
}