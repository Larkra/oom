using System;
using System.ComponentModel.DataAnnotations;

namespace Task3.Enums
{
    [Flags]
    public enum Platform
    {
        [Display(Name = "Unknown Platform", GroupName = "Unknown")]
        Unknown = 0,
        
        [Display(Name = "Personal Computer", GroupName = "Video Game")]
        PC = 1 << 0,

        [Display(Name = "PlayStation 3", GroupName = "Video Games")]
        PS3 = 1 << 1,

        [Display(Name = "PlayStation 4", GroupName = "Video Games")]
        PS4 = 1 << 2,

        [Display(Name = "XBox 360", GroupName = "Video Games")]
        XBox360 = 1 << 3,

        [Display(Name = "XBox One", GroupName = "Video Games")]
        XBoxOne = 1 << 4,

        [Display(Name = "Nintendo 64", GroupName = "Video Games")]
        Nintendo64 = 1 << 5
    }
}
