using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurpriseTask.Repository.Helpers;

public class Enums
{
    public enum RolesEnum
    {
        Admin = 1,
        User = 2
    }

    public enum CourseStatus{
        Pending = 1,
        Complete = 2
    }
}
