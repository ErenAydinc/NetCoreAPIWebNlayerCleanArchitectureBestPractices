using App.Application;
using App.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanApp.API.Filters
{
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Action method çalışmadan önce


            var a = context.ActionArguments.Values.FirstOrDefault();
            var idAsObject = a.GetType().GetProperty("Id")!.GetValue(a);

            if (idAsObject is not TId id)
            {
                await next();
                return;
            }
            if (await genericRepository.AnyAsync(id))
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;

            //Action method name
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult<T>.Fail($"Data not found.({entityName})({actionName})");
            context.Result = new NotFoundObjectResult(result);
            return;


            //Action method çalıştıktan sonra
        }
    }
}
