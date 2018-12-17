using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [PermController("会员管理")]
    public class UserController : Controller
    {
        #region 构造函数注入
        private int pageSize = 10;
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        #endregion

        #region 用户列表
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(long? levelId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            var result = await userService.GetModelListAsync(keyword, startTime, endTime, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        #endregion

        #region 添加用户
        [PermAction]
        public async Task<IActionResult> Add(string name, string password)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户名不能为空" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "登录密码不能为空" });
            }
            long id = await userService.AddAsync(name, password, null, null);
            if (id <= 0)
            {
                if (id == -1)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "用户名已经存在" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "会员添加失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "会员添加成功" });
        }

        public async Task<IActionResult> Check(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new AjaxResult { Status = 0, Msg = "用户名不能为空" });
            }
            long id = await userService.CheckUserNameAsync(name);
            return Json(new AjaxResult { Status = 1, Msg = "检测成功", Data = id });
        }
        #endregion

        #region 修改密码
        [PermAction("修改密码")]
        public async Task<IActionResult> EditPwd(long id, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "登录密码不能为空" });
            }
            long res = await userService.ResetPasswordAsync(id, password);
            if (res <= 0)
            {
                if (id == -1)
                {
                    return Json(new AjaxResult { Status = 0, Msg = "用户不存在" });
                }
                return Json(new AjaxResult { Status = 0, Msg = "重置密码失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "重置密码成功" });
        }
        #endregion

        #region 冻结用户
        [PermAction("冻结用户")]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await userService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻用户失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻用户成功" });
        }
        #endregion

        #region 删除用户
        [PermAction("删除用户")]
        public async Task<IActionResult> Del(long id)
        {
            long res = await userService.DeleteAsync(id);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除用户失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除用户成功" });
        }
        #endregion
    }
}