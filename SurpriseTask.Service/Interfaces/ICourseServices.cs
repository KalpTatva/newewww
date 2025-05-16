using SurpriseTask.Repository.ViewModels;

namespace SurpriseTask.Service.Interfaces;

public interface ICourseServices
{
    Task<CoursesViewModel> GetCourses(string? searchTerm = null,int pageNumber = 1, int pageSize = 5);
    Task<bool> AddCourse(AddCourseViewModel model);
    Task<bool> EditCourse(AddCourseViewModel model);
    Task<string> EnrollInCourse(int CourseId, string Email);
    Task<EnrolledCourseViewModel> GetEnrolmentDetails(string Email);
    Task<bool> CompleteCourse(int CourseId,string Email);
    Task<string> DeleteCourse(int CourseId,string Email);

}

