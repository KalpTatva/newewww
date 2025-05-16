using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurpriseTask.Repository.Models;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    [Required]
    public string? CourseName {get;set;}

    [Required]
    public string? CourseContent {get;set;}

    [Required]
    public int Credits {get;set;}

    [Required]
    public string? Department {get;set;}

    [Required]
    public bool IsDeleted {get;set;} 


    public string? CreatedAt { get; set; }

    public string? EditedAt { get; set; }

    public int? CreatedById { get; set; }

    public int? EditedById { get; set; }

    public int? DeletedById { get; set; }


    public string? DeletedAt { get; set; }

    public ICollection<UserCourseMapping> UserCourseMapping { get; set; } = new List<UserCourseMapping>();


}
