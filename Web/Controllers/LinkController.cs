using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LinkController : Controller
    {
        #region 构造函数注入
        private readonly ILinkService linkService;
        private int pageSize = 10;
        public LinkController(ILinkService linkService)
        {
            this.linkService = linkService;
        }
        #endregion

        #region 系统图片设置列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex=1)
        {
            var res = await linkService.GetModelListAsync(keyword,startTime,endTime,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        #endregion

        #region 添加系统图片
        [HttpPost]
        public async Task<IActionResult> Add(string name, string imgUrl, string url,int sort)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "系统图片名不能为空" });
            }
            string res;
            bool flag = ImageHelper.Base64Save(imgUrl, out res);
            if(!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = res });
            }
            long id = await linkService.AddAsync(name, res, url, sort);
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加系统图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加系统图片成功", Data = "/admin/link/list" });
        }
        #endregion

        #region 修改系统图片
        [HttpPost]
        public async Task<IActionResult> Edit(long id, string name,string imgUrl,string url, int sort)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "系统图片名不能为空" });
            }
            string res;
            bool flag = ImageHelper.Base64Save(imgUrl, out res);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = res });
            }
            var result = await linkService.EditAsync(id, name, res, url, sort);
            if (result <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改系统图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改系统图片成功" });
        }
        [HttpPost]
        public async Task<IActionResult> GetLink(long id)
        {
            var model = await linkService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结系统图片
        [HttpPost]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await linkService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻系统图片操作失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻系统图片操作成功" });
        }
        #endregion

        #region 删除系统图片
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await linkService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除系统图片失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除系统图片成功" });
        }
        #endregion
    }
}