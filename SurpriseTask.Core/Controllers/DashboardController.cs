using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurpriseTask.Repository.ViewModels;
using SurpriseTask.Service.Interfaces;

namespace SurpriseTask.Core.Controllers;

public class DashboardController : Controller
{

    private readonly IUserService _userService;
    private readonly ICourseServices _courseService;
    public DashboardController( 
        IUserService userService,
        ICourseServices courseService
        )
    {
        _userService = userService;
        _courseService = courseService;
    }

    #region Admin
    [CustomAuthorizationFilter("Admin")]
    public IActionResult Index()
    {
        return View();
    }

    // get all courses

    [HttpGet]
    public async Task<IActionResult> GetAllCourses(string? searchTerm = null,int pageNumber = 1, int pageSize = 5)
    {
        string RoleName = HttpContext.Items["UserRole"] as string ?? string.Empty;
        SurpriseTask.Repository.ViewModels.CoursesViewModel coursesViewModel = await _courseService.GetCourses(searchTerm,pageNumber,pageSize);
        coursesViewModel.Role = RoleName;
        return PartialView("_CoursePartial",coursesViewModel);
        
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCourseForm(AddCourseViewModel model)
    {
        try{
            bool res = await _courseService.AddCourse(model);
            if(res)
            {
                return Json(new {
                    success = true,
                    message = "Course added successfully"
                });
            }
            return Json(new {
                success = false,
                message = "Error Occured while adding course : 500"
            });
        }catch(Exception e){
            return Json(new {
                success = false,
                message = e.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditCourseForm(AddCourseViewModel model)
    {
        try{
            bool res = await _courseService.EditCourse(model);
            if(res)
            {
                return Json(new {
                    success = true,
                    message = "Course Edited successfully"
                });
            }
            return Json(new {
                success = false,
                message = "Error Occured while Edit course : 500"
            });
        }catch(Exception e){
            return Json(new {
                success = false,
                message = e.Message
            });
        }
    }


    #endregion
    #region User

    [CustomAuthorizationFilter("User","Admin")]
    public IActionResult UserDashBoard()
    {
        return View();
    }

    [CustomAuthorizationFilter("User")]
    public IActionResult EnrolledIn()
    {
        return View();
    }

    public async Task<IActionResult> GetEnrolmentDetails()
    {
        string Email = HttpContext.Items["UserEmail"] as string ?? string.Empty;
        EnrolledCourseViewModel enrolledCourseViewModel = await _courseService.GetEnrolmentDetails(Email);
        return PartialView("_EnrolledCoursePartial",enrolledCourseViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EnrollInCourse(int CourseId)
    {
        try{
            string Email = HttpContext.Items["UserEmail"] as string ?? string.Empty;
            string res = await _courseService.EnrollInCourse(CourseId,Email);
            if(res == "success")
            {
                return Json(new {
                    success = true,
                    message = "Enrolled in course successfully!"
                });
            }
            return Json(new {
                success = false,
                message = res
            });
        }catch(Exception e){
            return Json(new {
                success = false,
                message = e.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> CompleteCourse(int CourseId)
    {
        try{
            string Email = HttpContext.Items["UserEmail"] as string ?? string.Empty;
            bool res = await _courseService.CompleteCourse(CourseId,Email);
            if(res)
            {
                return Json(new {
                    success = true,
                    message = "Course Completed successfully"
                });
            }
            return Json(new {
                success = false,
                message = "Error Occured while Completing course : 500"
            });

        }catch(Exception e){
            return Json(new {
                success = false,
                message = e.Message
            });
        }
    }



    [HttpPost]
    public async Task<IActionResult> DeleteCourse(int CourseId)
    {
        try{
            string Email = HttpContext.Items["UserEmail"] as string ?? string.Empty;

            string res = await _courseService.DeleteCourse(CourseId,Email);
            if(res == "success")
            {
                return Json(new {
                    success = true,
                    message = "Course Completed successfully"
                });
            }
            return Json(new {
                success = false,
                message = res
            });

        }catch(Exception e){
            return Json(new {
                success = false,
                message = e.Message
            });
        }
    }



    #endregion

}

internal class CoursesViewModel
{
}