using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PortalRandkowy.API.Data;
using Microsoft.Extensions.DependencyInjection;

namespace PortalRandkowy.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)   
         // 1 parametr mowi o tym co chcemy zrobic podczas akcji a drugi pozwala uruchomic kodu po wykonaniu akcji
        {
            var resultContext = await next();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();     // pobieramy repo


            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);       //pobieramy dla uzytk id
            var user = await repo.GetUser(userId);  // pobieramy uzytk
            user.LastActive = DateTime.Now;     // wpisujemy kodzine po wykokaniu kodu

            await repo.SaveAll();

        }
    }
}