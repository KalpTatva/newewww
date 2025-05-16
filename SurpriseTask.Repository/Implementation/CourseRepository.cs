using Microsoft.Extensions.Configuration;
using SurpriseTask.Repository.Interfaces;
using SurpriseTask.Repository.Models;
using Microsoft.EntityFrameworkCore;
using SurpriseTask.Repository.ViewModels;

namespace SurpriseTask.Repository.Implementation;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;       
    }


    public async Task<List<Course>?> GetCourses(string? searchTerm = null,int pageNumber = 1, int pageSize = 5)
    {
        List<Course>? courses = new(); 
        if(searchTerm!=null)
        {
            courses = await _context.Courses
                .Where(x => x != null && x.CourseName != null && x.CourseName.ToLower().Trim().Contains(searchTerm) && x.IsDeleted == false)
                .OrderBy(x => x.CourseId).ToListAsync();
        }
        else{
            courses = await _context.Courses.Where(x => x.IsDeleted == false).OrderBy(x => x.CourseId).ToListAsync();
        }
        
        courses = courses.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    
        return courses;
    }

    public async Task<int> GetCourseCount()
    {
        return await _context.Courses.Where(x => x.IsDeleted == false).CountAsync();
    }


    public async Task<Course?> GetCourseById(int CourseId)
    {
        return await _context.Courses.Where(x => x.CourseId == CourseId).FirstOrDefaultAsync();
    }
    public async Task AddCourse(Course course)
    {
        _context.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task AddMapping(UserCourseMapping course)
    {
        _context.Add(course);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateMapping(UserCourseMapping course)
    {
        _context.Update(course);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCourse(Course course)
    {
        _context.Update(course);
        await _context.SaveChangesAsync();
    }



    public async Task<UserCourseMapping?> GetCourseByIds(int CourseId, int UserId)
    {
        return await _context.UserCourseMapping.Where(x => x.CourseId == CourseId && x.UserId == UserId).FirstOrDefaultAsync();
    }

    public async Task<UserCourseMapping?> GetCourseMappingByCourseId(int CourseId)
    {
        return await _context.UserCourseMapping.Where(x => x.CourseId == CourseId).FirstOrDefaultAsync(); 
    }


    public async Task<List<EnrolledIn>> GetEnrolmentDetails(int userid)
    {
        List<EnrolledIn> enrolledCourses = await (
                from uc in _context.UserCourseMapping
                join u in _context.Users on uc.UserId equals u.UserId
                join c in _context.Courses on uc.CourseId equals c.CourseId
                where uc.UserId == userid

                select new EnrolledIn
                {
                    CourseName = c.CourseName,
                    CourseId = c.CourseId,
                    CourseContent = c.CourseContent,
                    Credits = c.Credits,
                    UserId = u.UserId,
                    Department = c.Department,
                    status = uc.CourseStatus
                }
            ).ToListAsync();

        return enrolledCourses;
        
    }

    public async Task<UserCourseMapping?> GetCourseMappingByIds(int CourseId, int UserId)
    {
        return await _context.UserCourseMapping.Where(x => x.CourseId == CourseId && x.UserId == UserId).FirstOrDefaultAsync();
    }
}

