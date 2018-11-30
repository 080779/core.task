using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using DTO;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models.Admin;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AdminController : Controller
    {
        #region 构造函数注入
        private readonly IAdminService adminService;
        private readonly IPermissionService permissionService;
        private readonly IPermissionTypeService permissionTypeService;
        private readonly int pageSize = 10;
        public AdminController(IAdminService adminService, IPermissionService permissionService, IPermissionTypeService permissionTypeService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
            this.permissionTypeService = permissionTypeService;
        }
        #endregion

        #region 管理员列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            AdminSearchResult res = await adminService.GetModelListAsync("admin", keyword, startTime, endTime, pageIndex, pageSize);
            ListViewModel model = new ListViewModel();
            model.Admins = res.Admins;
            PermissionTypeDTO[] types = await permissionTypeService.GetModelListIsEnableAsync();
            List<PermissionType> permissionTypes = new List<PermissionType>();
            foreach (var type in types)
            {
                PermissionType permissionType = new PermissionType();
                permissionType.Name = type.Name;
                PermissionDTO[] permissions = await permissionService.GetByTypeIdAsync(type.Id);
                permissionType.Permissions = permissions.ToList();
                permissionTypes.Add(permissionType);
            }
            model.PermissionTypes = permissionTypes;
            model.PageCount = res.PageCount;
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加管理员
        [HttpPost]
        public async Task<IActionResult> Add(string name, string mobile, string password)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员账号不能为空" });
            }
            if (!Regex.IsMatch(mobile, @"^1\d{10}$"))
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员手机号格式不正确" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员密码不能为空" });
            }
            long id = await adminService.AddAsync(name, mobile, null, password);
            if (id <= 0)
            {
                if (id == -1)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "管理员账号已经存在" });
                }
                if (id == -2)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "管理员手机号已经存在" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "添加管理员失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加管理员成功", Data = "/admin/admin/list" });
        }
        #endregion

        #region 修改管理员密码
        [HttpPost]
        public async Task<IActionResult> EditPassword(long id, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员密码不能为空" });
            }
            bool res = await adminService.UpdateAsync(id, password);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员密码修改失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "管理员密码修改成功", Data = "/admin/admin/list" });
        }
        #endregion

        #region 修改管理员权限
        [HttpPost]
        public async Task<IActionResult> EditPermission(long id, List<long> permissionIds)
        {
            if (permissionIds == null)
            {
                permissionIds = new List<long>();
            }
            bool res = await adminService.UpdateAsync(id, permissionIds);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "编管理员权限失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "编辑管理员权限成功", Data = "/admin/admin/list" });
        }

        public async Task<IActionResult> GetPerm(List<long> permissionIds)
        {
            if (permissionIds == null)
            {
                permissionIds = new List<long>();
            }
            PermissionTypeDTO[] types = await permissionTypeService.GetModelListIsEnableAsync();
            List<PermissionType> permissionTypes = new List<PermissionType>();
            foreach (var type in types)
            {
                PermissionType permissionType = new PermissionType();
                permissionType.Name = type.Name;
                PermissionDTO[] permissions = await permissionService.GetByTypeIdAsync(type.Id);
                foreach (var perm in permissions)
                {
                    if (permissionIds.Contains(perm.Id))
                    {
                        perm.IsChecked = true;
                    }
                }
                permissionType.Permissions = permissions.ToList();
                permissionTypes.Add(permissionType);
            }
            return Json(new AjaxResult { Status = 1, Data = permissionTypes });
        }
        #endregion

        #region 冻结管理员账号
        [HttpPost]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await adminService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻管理员账号操作失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻管理员账号操作成功" });
        }
        #endregion

        #region 删除管理员账号
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await adminService.DeleteAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除管理员账号失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除管理员账号成功" });
        }
        #endregion
    }
}