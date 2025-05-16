namespace SurpriseTask.Repository.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId {get;set;}
    public string? UserName {get;set;}

    [Required]
    public string? UserEmail {get;set;}

    [Required]
    public string? password {get;set;}
    public int RoleId {get;set;}
    public ICollection<UserCourseMapping> UserCourseMapping { get; set; } = new List<UserCourseMapping>();

}
