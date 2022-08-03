using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserManager.Filters
{
    public class ExceptionFilter : 
        IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new BadRequestResult();
        }
    }
}