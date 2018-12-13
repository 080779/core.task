﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class LinkController : Controller
    {
        #region 构造函数注入
        private readonly ILinkService linkService;
        private readonly ILinkTypeService linkTypeService;
        public LinkController(ILinkService linkService, ILinkTypeService linkTypeService)
        {
            this.linkService = linkService;
            this.linkTypeService = linkTypeService;
        }
        #endregion

        #region 图片类别列表
        [HttpGet]
        public IActionResult TypeList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TypeList(bool flag = true)
        {
            var model = await linkTypeService.GetModelListAsync();
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加图片类别
        [HttpPost]
        public async Task<IActionResult> AddType(string name, string description)
        {
            long res = await linkTypeService.AddAsync(name, description);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加图片类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加图片类别成功", Data = "/admin/idname/typelist" });
        }
        #endregion

        #region 修改图片类别
        [HttpPost]
        public async Task<IActionResult> EditType(long id, string name, string description)
        {
            long res = await linkTypeService.EditAsync(id, name, description);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改图片类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改图片类别成功", Data = "/admin/idname/typelist" });
        }

        [HttpPost]
        public async Task<IActionResult> GetType(long id)
        {
            var model = await linkTypeService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻图片类别
        [HttpPost]
        public async Task<IActionResult> FrozenType(long id)
        {
            bool res = await linkTypeService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻图片类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻图片类别成功" });
        }
        #endregion

        #region 删除图片类别
        [HttpPost]
        public async Task<IActionResult> DelType(long id)
        {
            bool res = await linkTypeService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除图片类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除图片类别成功" });
        }
        #endregion

        #region 图片列表
        [HttpGet]
        public IActionResult List(long typeId)
        {
            return View(typeId);
        }
        [HttpPost]
        public async Task<IActionResult> List(long typeId, bool flag = true)
        {
            var res = await linkService.GetByTypeIdAsync(typeId);
            return Json(new AjaxResult { Status = 1, Data = res });
        }
        #endregion

        #region 添加图片
        [HttpPost]
        public async Task<IActionResult> Add(long typeId, string name, string imgUrl, string url,int sort)
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
            var model = await linkService.GetModelByIdAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结图片
        [HttpPost]
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
    }
}