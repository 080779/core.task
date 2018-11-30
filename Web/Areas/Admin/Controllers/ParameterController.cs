using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ParameterController : Controller
    {
        #region 构造函数注入
        private readonly IParameterTypeService parameterTypeService;
        private readonly IParameterService parameterService;
        public ParameterController(IParameterTypeService parameterTypeService, IParameterService parameterService)
        {
            this.parameterTypeService = parameterTypeService;
            this.parameterService = parameterService;
        }
        #endregion

        #region 参数类别列表
        [HttpGet]
        public IActionResult TypeList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TypeList(bool flag = true)
        {
            var model = await parameterTypeService.GetModelListAsync();
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加参数类别
        [HttpPost]
        public async Task<IActionResult> AddType(string name, string description, int sort)
        {
            long res = await parameterTypeService.AddAsync(name, description, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加权限参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加权限参数成功", Data = "/admin/idname/typelist" });
        }
        #endregion

        #region 修改参数类别
        [HttpPost]
        public async Task<IActionResult> EditType(long id, string name, string description, int sort)
        {
            long res = await parameterTypeService.EditAsync(id, name, description, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改权限参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改权限参数成功", Data = "/admin/idname/typelist" });
        }

        [HttpPost]
        public async Task<IActionResult> GetType(long id)
        {
            var model = await parameterTypeService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻参数类别
        [HttpPost]
        public async Task<IActionResult> FrozenType(long id)
        {
            bool res = await parameterTypeService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻权限参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻权限参数成功" });
        }
        #endregion

        #region 删除参数类别
        [HttpPost]
        public async Task<IActionResult> DelType(long id)
        {
            bool res = await parameterTypeService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除参数类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除参数类别成功" });
        }
        #endregion

        #region 参数列表
        [HttpGet]
        public IActionResult List(long typeId)
        {
            return View(typeId);
        }
        [HttpPost]
        public async Task<IActionResult> List(long typeId, bool flag = true)
        {
            var model = await parameterService.GetByTypeIdAsync(typeId);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加参数
        [HttpPost]
        public async Task<IActionResult> Add(string name, string stringValue, decimal decimalValue, string remark, long typeId, int sort = 1)
        {
            var res = await parameterService.AddAsync(name, stringValue, decimalValue, remark, sort, typeId);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加参数成功" });
        }
        #endregion

        #region 修改参数
        [HttpPost]
        public async Task<IActionResult> Edit(long id, string name, string stringValue, decimal decimalValue, string remark, int sort)
        {
            var res = await parameterService.EditAsync(id, name, stringValue, decimalValue, remark, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改参数成功" });
        }
        [HttpPost]
        public async Task<IActionResult> GetParameter(long id)
        {
            var model = await parameterService.GetModelByIdAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻参数
        [HttpPost]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await parameterService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻参数成功" });
        }
        #endregion

        #region 删除参数
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await parameterService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除参数失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除参数成功" });
        }
        #endregion
    }
}