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
    [PermController("图片管理")]
    public class LinkController : Controller
    {
        #region 构造函数注入
        private readonly ILinkService linkService;
        private readonly int pageSize = 10;
        public LinkController(ILinkService linkService)
        {
            this.linkService = linkService;
        }
        #endregion

        #region 图片列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            var res = await linkService.GetModelListAsync(keyword,startTime,endTime,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        #endregion

        #region 添加图片
        [HttpPost]
        [PermAction("添加图片")]
        public async Task<IActionResult> Add(int typeId, string name, string imgUrl, string url,int sort)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "图片名不能为空" });
            }
            var res = await ImageHelper.Base64SaveAsync(imgUrl);
            if(!res.Key)
            {
                return Json(new AjaxResult { Status = 0, Msg = res.Value });
            }
            long id = await linkService.AddAsync(typeId, name, res.Value, url, sort);
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加图片成功", Data = "/admin/link/list" });
        }
        #endregion

        #region 修改图片
        [HttpPost]
        [PermAction("修改图片")]
        public async Task<IActionResult> Edit(long id, string name,string imgUrl,string url, int sort)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "图片名不能为空" });
            }

            var res = await ImageHelper.Base64SaveAsync(imgUrl);
            if (!res.Key)
            {
                return Json(new AjaxResult { Status = 0, Msg = res.Value });
            }

            var result = await linkService.EditAsync(id, name, res.Value, url, sort);
            if (result <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改图片成功" });
        }
        [HttpPost]
        public async Task<IActionResult> GetLink(long id)
        {
            var model = await linkService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结图片
        [HttpPost]
        [PermAction("冻结图片")]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await linkService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻图片操作失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻图片操作成功" });
        }
        #endregion

        #region 删除图片
        [HttpPost]
        [PermAction("删除图片")]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await linkService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除图片成功" });
        }
        #endregion

        #region 富文本编辑器上传图片
        //上传到本地服务器
        [HttpPost]
        public async Task<IActionResult> UpContentImg(IFormFile imgFile)
        {
            return Json(new { errno = "0", data = await ImageHelper.SaveAsync(imgFile) });
        }

        //上传到七牛云存储服务器
        //[HttpPost]
        //public async Task<IActionResult> UpContentImg(IFormFile imgFile)
        //{
        //    var res = await QiniuHelper.UploadStreamAsync(imgFile);
        //    if (res.Key != 200)
        //    {
        //        return Json(new { errno = res.Key.ToString(), data = new string[] { res.Value } });
        //    }
        //    return Json(new { errno = "0", data = new string[] { res.Value } });
        //}
        #endregion
    }
}