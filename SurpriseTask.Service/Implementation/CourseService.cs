using Microsoft.Extensions.Configuration;
using SurpriseTask.Repository.Interfaces;
using SurpriseTask.Repository.Models;
using SurpriseTask.Repository.ViewModels;
using SurpriseTask.Service.Interfaces;
using static SurpriseTask.Repository.Helpers.Enums;

namespace SurpriseTask.Service.Implementation;

public class CourseService : ICourseServices
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IConfiguration _configuration;
    public CourseService(
        IUserRepository userRepository,
        IConfiguration configuration,
        ICourseRepository courseRepository
        
        )
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _courseRepository = courseRepository;
    }



    public async Task<CoursesViewModel> GetCourses(string? searchTerm = null,int pageNumber = 1, int pageSize = 5)
    {
        try{
            CoursesViewModel coursesViewModel = new ();
            
            searchTerm = (searchTerm ?? "").ToLower().Trim();
            
            List<Course>? courses = await _courseRepository.GetCourses(searchTerm,pageNumber,pageSize);
            if(courses!=null)
            {
                int TotalItems = await _courseRepository.GetCourseCount(searchTerm);
                coursesViewModel.Courses = courses;
                coursesViewModel.TotalItems = TotalItems;
            }
            return coursesViewModel;
            
        }catch{
            throw;
        }
    }

    public async Task<bool> AddCourse(AddCourseViewModel model){
        try{
            Course course = new();
            
            if(model!=null){
                course.CourseName = model.CourseName;
                course.CourseContent = model.CourseContent;
                course.Credits = model.Credits;
                course.Department = model.Department;
                course.CreatedAt = DateTime.Now.ToString();
                course.EditedAt = DateTime.Now.ToString();

                await _courseRepository.AddCourse(course);
                return true;
            }

            return false;
        }catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> EditCourse(AddCourseViewModel model){
        try{
            Course? course = await _courseRepository.GetCourseById(model.CourseId);
            
            if(model!=null){
                course.CourseName = model.CourseName;
                course.CourseContent = model.CourseContent;
                course.Credits = model.Credits;
                course.Department = model.Department;
                course.CreatedAt = DateTime.Now.ToString();
                course.EditedAt = DateTime.Now.ToString();

                await _courseRepository.UpdateCourse(course);
                return true;
            }

            return false;
        }catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> EnrollInCourse(int CourseId, string Email)
    {
        try{
            Users? users = await _userRepository.GetUserByEmail(Email.Trim().ToLower());
            UserCourseMapping? userCourseMapping = users != null ? await _courseRepository.GetCourseByIds(CourseId,users.UserId) : new UserCourseMapping();

            if(userCourseMapping!=null)
            {
                return "Already enrolled in the course! check out in enrollment!";
            }
            else{
                UserCourseMapping userCourseMapping1 = new(){
                    UserId = users.UserId,
                    CourseId = CourseId,
                    CourseStatus = (int)CourseStatus.Pending
                };
                await _courseRepository.AddMapping(userCourseMapping1);
                return "success";
            }
        }catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    public async Task<EnrolledCourseViewModel> GetEnrolmentDetails(string Email)
    {
        try{
          Users? users = await _userRepository.GetUserByEmail(Email.Trim().ToLower()); 
          EnrolledCourseViewModel enrolledCourseViewModel = new(); 
          List<EnrolledIn> model = users != null ?  await _courseRepository.GetEnrolmentDetails(users.UserId) : new List<EnrolledIn>();
          if(model!=null)
          {
                enrolledCourseViewModel.enrolledIns = model;
          }

          return enrolledCourseViewModel;

        }catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> CompleteCourse(int CourseId,string Email)
    {
        try{
            Users? users = await _userRepository.GetUserByEmail(Email.Trim().ToLower()); 
            UserCourseMapping? course = users!=null ? await _courseRepository.GetCourseMappingByIds(CourseId, users.UserId) : new UserCourseMapping();

            if(course!=null && course.CourseStatus==1)
            {
                course.CourseStatus = (int)CourseStatus.Complete;
                await _courseRepository.UpdateMapping(course);
                return true;

            }
            return false;
            
        }catch(Exception e){
            throw new Exception(e.Message);
        }
    }
    public async Task<string> DeleteCourse(int CourseId,string Email)
    {
        try{
            Users? users = await _userRepository.GetUserByEmail(Email.Trim().ToLower()); 
            UserCourseMapping? course = await _courseRepository.GetCourseMappingByCourseId(CourseId) ;

            if(course!=null)
            {
                course.CourseStatus = (int)CourseStatus.Complete;
                await _courseRepository.UpdateMapping(course);
                return "Students are enrolled in this course, can not delete it!";
            }

            Course? courseToDelete = await _courseRepository.GetCourseById(CourseId);
            courseToDelete.IsDeleted=true;
            courseToDelete.DeletedAt = DateTime.Now.ToString();
            courseToDelete.DeletedById = users.UserId;
            await _courseRepository.UpdateCourse(courseToDelete);

            return "success";
            
        }catch(Exception e){
            throw new Exception(e.Message);
        }
    }

}
