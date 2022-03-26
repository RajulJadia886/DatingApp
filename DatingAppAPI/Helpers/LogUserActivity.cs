using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAppAPI.Extensions;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAppAPI.Helpers
{
    //action filter to log user activity such as last active property when the user interacts with any of the apis.
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            var user = await repo.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;

            await repo.SaveAllAsync();



        }
    }
}