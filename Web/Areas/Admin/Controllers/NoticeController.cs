using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [PermController("公告管理")]
    public class NoticeController : Controller
    {
        #region 构造函数注入
        private readonly INoticeService noticeService;
        private readonly int pageSize = 10;
        public NoticeController(INoticeService noticeService)
        {
            this.noticeService = noticeService;
        }
        #endregion

        #region 公告列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex=1)
        {
            var res = await noticeService.GetModelListAsync(keyword,startTime,endTime,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        #endregion

        #region 添加公告
        [HttpPost]
        [PermAction("添加公告")]
        public async Task<IActionResult> Add(string title,string content,int enabled)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Json(new AjaxResult { Status = 0, Msg = "公告标题不能为空" });
            }
            if (string.IsNullOrEmpty(content))
            {
                return Json(new AjaxResult { Status = 0, Msg = "公告内容不能为空" });
            }
            long adminId = Convert.ToInt64(HttpContext.Session.GetString("Platform_Admin_Id"));
            long id = await noticeService.AddAsync(title,content, enabled, adminId);
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加公告失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加公告成功" });
        }
        #endregion

        #region 修改公告
        [HttpPost]
        [PermAction("修改公告")]
        public async Task<IActionResult> Edit(long id, string title, string content, int enabled)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Json(new AjaxResult { Status = 0, Msg = "公告标题不能为空" });
            }

            var result = await noticeService.EditAsync(id, title, content, enabled);
            if (!result)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改公告失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改公告成功" });
        }
        #endregion

        #region 冻结公告
        [HttpPost]
        [PermAction("冻结公告")]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await noticeService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻公告操作失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻公告操作成功" });
        }
        #endregion

        #region 删除公告
        [HttpPost]
        [PermAction("删除公告")]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await noticeService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除公告失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除公告成功" });
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
            if (res.Key != 200)
            {
                return Json(new { errno = res.Key.ToString(), data = new string[] { res.Value } });
            }
            return Json(new { errno = "0", data = new string[] { res.Value } });
        }
        #endregion
    }
}