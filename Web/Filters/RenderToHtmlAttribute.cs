using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Filters
{
    public class RenderToHtmlAttribute: ActionFilterAttribute
    {
        public IServiceProvider serviceProvider { get; set; }
        public string Path { get; set; }
        public string Template { get; set; }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //await WriteViewToFileAsync(context);
            await base.OnResultExecutionAsync(context, next);
        }

        //private async Task WriteViewToFileAsync(ResultExecutingContext context)
        //{
        //    try
        //    {
        //        var html = await RenderToStringAsync(context);
        //        if (string.IsNullOrWhiteSpace(html))
        //            return;
        //        var path = @"d:/static";
        //        var directory = System.IO.Path.GetDirectoryName(path);
        //        if (string.IsNullOrWhiteSpace(directory))
        //            return;
        //        if (Directory.Exists(directory) == false)
        //            Directory.CreateDirectory(directory);
        //        File.WriteAllText(path, html);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //protected async Task<string> RenderToStringAsync(ResultExecutingContext context)
        //{
        //    string viewName = "";
        //    object model = null;
        //    if (context.Result is ViewResult result)
        //    {
        //        viewName = result.ViewName;
        //        model = result.Model;
        //    }
        //    //var razorViewEngine = Ioc.Create<IRazorViewEngine>();
        //    //var tempDataProvider = Ioc.Create<ITempDataProvider>();
        //    //var serviceProvider = Ioc.Create<IServiceProvider>();
        //    var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
        //    var actionContext = new ActionContext(httpContext, context.RouteData, new ActionDescriptor());
        //    using (var stringWriter = new StringWriter())
        //    {
        //        var viewResult = razorViewEngine.FindView(actionContext, viewName, true);
        //        if (viewResult.View == null)
        //            throw new ArgumentNullException($"未找到视图： {viewName}");
        //        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
        //        var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, new TempDataDictionary(actionContext.HttpContext, tempDataProvider), stringWriter, new HtmlHelperOptions());
        //        await viewResult.View.RenderAsync(viewContext);
        //        return stringWriter.ToString();
        //    }
        //    return "";
        //}
    }    
}
