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
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [PermController("管理员管理")]
    [Area("admin")]
    public class AdminController : Controller
    {
        #region 构造函数注入
        private readonly IAdminService adminService;
        private readonly IPermissionService permissionService;
        private readonly int pageSize = 10;
        public AdminController(IAdminService adminService, IPermissionService permissionService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
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
            AdminSearchResult res = await adminService.GetModelListAsync(keyword, startTime, endTime, pageIndex, pageSize);
            string[] types = await permissionService.GetModelTypeListIsEnableAsync();
            List<PermTypeModel> permTypes = new List<PermTypeModel>(); 
            ListViewModel model = new ListViewModel();
            model.List = res.List;
            foreach (var item in types)
            {
                PermTypeModel typeModel = new PermTypeModel();
                typeModel.TypeName = item;
                typeModel.Permissions = await permissionService.GetModelListIsEnableByTypeNameAsync(item);
                permTypes.Add(typeModel);
            }
            model.PermTypes = permTypes;
            model.PageCount = res.PageCount;
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加管理员
        [HttpPost]
        [PermAction("添加管理员")]
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
        [PermAction("修改管理员密码")]
        public async Task<IActionResult> EditPwd(long id, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员密码不能为空" });
            }
            bool res = await adminService.EditAsync(id, password);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "管理员密码修改失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "管理员密码修改成功", Data = "/admin/admin/list" });
        }
        #endregion

        #region 修改管理员权限
        [HttpPost]
        [PermAction("修改管理员权限")]
        public async Task<IActionResult> EditPerm(long id, List<long> permissionIds)
        {
            if (permissionIds == null)
            {
                permissionIds = new List<long>();
            }
            bool res = await adminService.EditAsync(id, permissionIds);
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

            string[] types = await permissionService.GetModelTypeListIsEnableAsync();
            List<PermTypeModel> permTypes = new List<PermTypeModel>();
            foreach (var item in types)
            {
                PermissionDTO[] permissions = await permissionService.GetModelListIsEnableByTypeNameAsync(item);
                PermTypeModel typeModel = new PermTypeModel();
                typeModel.TypeName = item;
                foreach (var perm in permissions)
                {
                    if(permissionIds.Contains(perm.Id))
                    {
                        perm.IsChecked = true;
                    }
                }
                typeModel.Permissions = permissions;
                permTypes.Add(typeModel);
            }
            return Json(new AjaxResult { Status = 1, Data = permTypes });
        }
        #endregion

        #region 冻结管理员账号
        [HttpPost]
        [PermAction("冻结管理员账号")]
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
        [PermAction("删除管理员账号")]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await adminService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除管理员账号失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除管理员账号成功" });
        }
        #endregion
    }
}