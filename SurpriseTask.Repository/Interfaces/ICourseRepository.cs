using SurpriseTask.Repository.Models;
using SurpriseTask.Repository.ViewModels;

namespace SurpriseTask.Repository.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>?> GetCourses(string? searchTerm = null,int pageNumber = 1, int pageSize = 5);
    Task<int> GetCourseCount();
    Task<Course?> GetCourseById(int CourseId);
    Task AddCourse(Course course);
    Task UpdateCourse(Course course);
    Task<UserCourseMapping?> GetCourseByIds(int CourseId, int UserId);
    Task AddMapping(UserCourseMapping course);
    Task UpdateMapping(UserCourseMapping course);
    Task<List<EnrolledIn>> GetEnrolmentDetails(int userid);
    Task<UserCourseMapping?> GetCourseMappingByIds(int CourseId, int UserId);
    Task<UserCourseMapping?> GetCourseMappingByCourseId(int CourseId);
}
