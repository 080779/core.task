using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DTO;
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
        #endregion

        #region 修改密码
        [PermAction("修改密码")]
        public async Task<IActionResult> EditPwd(long id, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new AjaxResult { Status = 0, Msg = "登录密码不能为空" });
            }
            long res = await userService.EditPwdAsync(id, password);
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
            long res = await userService.DelAsync(id);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除用户失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除用户成功" });
        }
        #endregion

        #region 查看直接推荐图
        [HttpGet]
        [PermAction("查看直接推荐图")]
        public IActionResult MemberTree(long id=1)
        {
            return View(id);
        }
        //获取数据
        public async Task<string> Get(long uid, string id)
        {
            StringBuilder sb = new StringBuilder();
            if (id != "#")
            {
                uid = long.Parse(id);
            }
            MemberTreeDTO user;
            MemberTreeDTO[] list;
            string treeText;
            if (uid != -1)
            {
                if (uid != 0)
                {
                    user = await userService.GetMemberTreeModelAsync(uid);
                    list = await userService.GetMemberTreeListAsync(user.Id);
                    treeText = Treetext(user.Mobile, user.Amount, user.LevelName, user.Count);
                }
                else
                {
                    list = new MemberTreeDTO[0];
                    treeText = "查询无结果";
                }
            }
            else
            {
                list = new MemberTreeDTO[0];
                treeText = "查询无结果";
            }
            if (id == "#")
            {
                sb.Append("\"text\":\"" + treeText + "\",\"expanded\":\"false\",\"state\":{\"opened\":\"true\"}");
            }
            if (list.Count() > 0)
            {
                if (id == "#")
                {
                    sb.Append(",\"children\":[{");
                }

                for (int i = 0; i < list.Count(); i++)
                {

                    var list2 = await userService.GetMemberTreeListAsync(list[i].Id);
                    if (list2.Count() > 0)
                    {
                        sb.Append("\"text\":\"" + Treetext(list[i].Mobile, list[i].Amount, list[i].LevelName, list[i].Count) + "\",\"children\":true,\"id\":\"" + list[i].Id + "\"");
                    }
                    else
                    {
                        sb.Append("\"text\":\"" + Treetext(list[i].Mobile, list[i].Amount, list[i].LevelName, list[i].Count) + "\"");
                    }

                    if (i != list.Count() - 1)
                    {
                        sb.AppendLine("},{");
                    }
                }
                if (id == "#")
                {
                    sb.Append("}]");
                }
            }
            return "[{" + sb.ToString() + "}]";
        }
        //根据手机号查询
        public async Task<string> Search(string mobile, string token, string id)
        {
            //if (string.IsNullOrEmpty(token))
            //{
            //    return "token不能为空";
            //}

            //if (!Valid(token))
            //{
            //    return "token_invalid";
            //}
            long res;
            if (!string.IsNullOrEmpty(mobile))
            {
                res = await userService.GetIdByMobileAsync(mobile);
                if (res <= 0)
                {
                    res = -1;
                }
            }
            else
            {
                res = 1;
            }
            return await Get(res, id);
        }
        //内容格式
        private string Treetext(string mobile, decimal amount, string levelName, long count)
        {
            string treeText = "";
            treeText = "<span style='color:green;'>" + mobile + "</span>|<span style='color:green;'>" + levelName + "</span>|<span style='color:green;'>" + amount + "</span>|<span style='color:green;'>" + count + "</span> ";
            return treeText;
        }
        //token验证
        private bool Valid(string token)
        {
            string KEY = "321wqeewqwqsdaewq";
            string validToken = CommonHelper.GetMD5(DateTime.Now.ToString("yyyyMMdd") + KEY).ToLower();
            if (token != validToken)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}