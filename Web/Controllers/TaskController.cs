using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using Web.Filters;

namespace Web.Controllers
{
    public class TaskController : Controller
    {
        #region 构造函数注入
        private readonly ITaskService taskService;
        private IServiceProvider serviceProvider;
        private readonly int pageSize = 10;
        public TaskController(ITaskService taskService, IServiceProvider serviceProvider)
        {
            this.taskService = taskService;
            this.serviceProvider = serviceProvider;
        }
        #endregion

        #region 任务列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            var result = await taskService.GetModelListAsync(null, keyword, startTime, endTime, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        #endregion

        #region 添加任务
        [HttpPost]
        public async Task<IActionResult> Add(string title, decimal bonus, string condition, string explain, string content, DateTime endTime)
        {
            //long adminId = Convert.ToInt64(Session["Platform_AdminUserId"]);
            //string adminMobile = await adminService.GetMobileByIdAsync(adminId);
            string adminMobile = "admin";
            long id = await taskService.AddAsync(title, bonus, condition, explain, content, endTime, adminMobile);
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加任务成功" });
        }
        #endregion

        #region 修改任务
        [HttpPost]
        public async Task<IActionResult> Edit(long id, string title, decimal bonus, string condition, string explain, string content, DateTime endTime)
        {
            bool flag = await taskService.EditAsync(id, title, bonus, condition, explain, content, endTime);

            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改任务成功" });
        }
        [HttpPost]
        public async Task<ActionResult> GetModel(long id)
        {
            var res = await taskService.GetModelAsync(id, 0);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        #endregion

        #region 删除任务
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool flag = await taskService.DelAsync(id);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除任务失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除任务成功" });
        }
        #endregion

        #region 富文本编辑器上传图片
        //上传到本地服务器
        //[HttpPost]
        //public async Task<IActionResult> UpContentImg(IFormFile imgFile)
        //{            
            //return Json(new { errno = "0", data = await ImageHelper.SaveAsync(imgFile)});
        //}

        //上传到七牛云存储服务器
        [HttpPost]
        public async Task<IActionResult> UpContentImg(IFormFile imgFile)
        {
            var res = await QiniuHelper.UploadStreamAsync(imgFile);
            if(res.Key!=200)
            {
                return Json(new { errno = res.Key.ToString(), data = new string[] { res.Value } });
            }
            return Json(new { errno = "0", data = new string[] { res.Value } });
        }
        #endregion

        #region 页面静态化，未能实现，后面去做
        //页面静态化方法
        public async Task<string> RenderViewToString(string viewName, object model = null)
        {
            var actionContext = new ActionContext(this.HttpContext, this.RouteData, new ActionDescriptor());
            var razorViewEngine = serviceProvider.GetServices<IRazorViewEngine>().First();
            var tempDataProvider = serviceProvider.GetServices<ITempDataProvider>().First();
            using (var stringWriter = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, true);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"未找到视图： {viewName}");
                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
                var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, new TempDataDictionary(actionContext.HttpContext, tempDataProvider), stringWriter, new HtmlHelperOptions());
                await viewResult.View.RenderAsync(viewContext);
                return stringWriter.ToString();
            }
        }

        public IActionResult Info()
        {
            return View(10086);
        }
        #endregion
    }
}