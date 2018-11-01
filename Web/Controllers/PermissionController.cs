using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PermissionController : Controller
    {
        #region 构造函数注入
        private readonly IPermissionTypeService permissionTypeService;
        private readonly IPermissionService permissionService;
        public PermissionController(IPermissionTypeService permissionTypeService, IPermissionService permissionService)
        {
            this.permissionTypeService = permissionTypeService;
            this.permissionService = permissionService;
        }
        #endregion

        #region 权限类别列表
        [HttpGet]
        public IActionResult TypeList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TypeList(bool flag = true)
        {
            var model = await permissionTypeService.GetModelListAsync();
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加权限类别
        [HttpPost]
        public async Task<IActionResult> AddType(string name, string description, int sort = 1)
        {
            long res = await permissionTypeService.AddAsync(name,description,sort);
            if(res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg="添加权限类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加权限类别成功" ,Data = "/admin/permission/typelist" });
        }
        #endregion

        #region 修改权限类别
        [HttpPost]
        public async Task<IActionResult> EditType(long id, string name, string description, int sort = 1)
        {
            long res = await permissionTypeService.EditAsync(id, name, description, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改权限类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改权限类别成功", Data = "/admin/permission/typelist" });
        }

        [HttpPost]
        public async Task<IActionResult> GetType(long id)
        {
            var model = await permissionTypeService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻权限类别
        [HttpPost]
        public async Task<IActionResult> FrozenType(long id)
        {
            bool res = await permissionTypeService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻权限类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻权限类别成功" });
        }
        #endregion

        #region 删除权限类别
        [HttpPost]
        public async Task<IActionResult> DelType(long id)
        {
            bool res = await permissionTypeService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除权限类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除权限类别成功" });
        }
        #endregion

        #region 权限列表
        [HttpGet]
        public IActionResult List(long typeId)
        {
            return View(typeId);
        }
        [HttpPost]
        public async Task<IActionResult> List(long typeId, bool flag = true)
        {
            var model = await permissionService.GetByTypeIdAsync(typeId);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加权限
        [HttpPost]
        public async Task<IActionResult> Add(string name,long permissionTypeId, int sort=1)
        {
            var res = await permissionService.AddAsync(name,sort,permissionTypeId);
            if(res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加权限失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加权限失败" });
        }
        #endregion

        #region 修改权限
        [HttpPost]
        public async Task<IActionResult> Edit(long id, string name, int sort)
        {
            var res = await permissionService.EditAsync(id, name, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改权限失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改权限成功" });
        }
        [HttpPost]
        public async Task<IActionResult> GetPermission(long id)
        {
            var model = await permissionService.GetModelByIdAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻权限
        [HttpPost]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await permissionService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻权限失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻权限成功" });
        }
        #endregion

        #region 删除权限
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await permissionService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除权限失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除权限成功" });
        }
        #endregion
    }
}