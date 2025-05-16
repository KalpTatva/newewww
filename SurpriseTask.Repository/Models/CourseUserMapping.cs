using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurpriseTask.Repository.Models;

public class UserCourseMapping
{
    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MappingId { get; set; }

    [Key]
    [Column(Order = 2)]
    public int UserId { get; set; }

    [Key]
    [Column(Order = 3)]
    public int CourseId { get; set; }

    [Key]
    [Column(Order = 4)]
    public int CourseStatus { get; set; }

    [ForeignKey("UserId")]
    public Users Users { get; set; }

    [ForeignKey("CourseId")]
    public Course Courses { get; set; }
}
