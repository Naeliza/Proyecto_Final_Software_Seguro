using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Final_Software_Seguro.Filters
{
    public class HtmlEncodingFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MinValue;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No es necesario hacer nada antes de que se ejecute la acción
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ViewResult)
            {
                var htmlEncoder = context.HttpContext.RequestServices.GetRequiredService<HtmlEncoder>();
                var viewResult = (ViewResult)context.Result;
                viewResult.ViewData.Model = EncodeModel(viewResult.ViewData.Model, htmlEncoder);
            }
        }

        private object EncodeModel(object model, HtmlEncoder htmlEncoder)
        {

            if (model is string)
            {
                return htmlEncoder.Encode((string)model);
            }

            // Si el modelo no es una cadena, simplemente lo devolvemos sin modificar
            return model;
        }
    }
}
