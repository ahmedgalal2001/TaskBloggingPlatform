using Microsoft.AspNetCore.Authorization;

public class AdminAttribute : AuthorizeAttribute
{
    public AdminAttribute()
    {
        Policy = "Admin";
    }
}