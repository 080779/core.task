using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class IdNameController : Controller
    {
        #region 构造函数注入
        private readonly IIdNameTypeService idNameTypeService;
        private readonly IIdNameService idNameService;
        public IdNameController(IIdNameTypeService idNameTypeService, IIdNameService idNameService)
        {
            this.idNameTypeService = idNameTypeService;
            this.idNameService = idNameService;
        }
        #endregion

        #region IdName类别列表
        [HttpGet]
        public IActionResult TypeList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TypeList(bool flag = true)
        {
            var model = await idNameTypeService.GetModelListAsync();
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加IdName类别
        [HttpPost]
        public async Task<IActionResult> AddType(string name, string description)
        {
            long res = await idNameTypeService.AddAsync(name,description);
            if(res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg= "添加权限IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加权限IdName成功", Data = "/admin/idname/typelist" });
        }
        #endregion

        #region 修改IdName类别
        [HttpPost]
        public async Task<IActionResult> EditType(long id, string name, string description)
        {
            long res = await idNameTypeService.EditAsync(id, name, description);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改权限IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改权限IdName成功", Data = "/admin/idname/typelist" });
        }

        [HttpPost]
        public async Task<IActionResult> GetType(long id)
        {
            var model = await idNameTypeService.GetModelAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻IdName类别
        [HttpPost]
        public async Task<IActionResult> FrozenType(long id)
        {
            bool res = await idNameTypeService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻权限IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻权限IdName成功" });
        }
        #endregion

        #region 删除IdName类别
        [HttpPost]
        public async Task<IActionResult> DelType(long id)
        {
            bool res = await idNameTypeService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除IdName类别失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除IdName类别成功" });
        }
        #endregion

        #region IdName列表
        [HttpGet]
        public IActionResult List(long typeId)
        {
            return View(typeId);
        }
        [HttpPost]
        public async Task<IActionResult> List(long typeId, bool flag = true)
        {
            var model = await idNameService.GetByTypeIdAsync(typeId);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 添加IdName
        [HttpPost]
        public async Task<IActionResult> Add(string name, string description,long typeId, int sort=1)
        {
            var res = await idNameService.AddAsync(name, description, sort, typeId);
            if(res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加IdName成功" });
        }
        #endregion

        #region 修改IdName
        [HttpPost]
        public async Task<IActionResult> Edit(long id, string name,string description, int sort)
        {
            var res = await idNameService.EditAsync(id, name, description, sort);
            if (res <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "修改IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "修改IdName成功" });
        }
        [HttpPost]
        public async Task<IActionResult> GetPermission(long id)
        {
            var model = await idNameService.GetModelByIdAsync(id);
            return Json(new AjaxResult { Status = 1, Data = model });
        }
        #endregion

        #region 冻结、解冻IdName
        [HttpPost]
        public async Task<IActionResult> Frozen(long id)
        {
            bool res = await idNameService.FrozenAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "冻结、解冻IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "冻结、解冻IdName成功" });
        }
        #endregion

        #region 删除IdName
        [HttpPost]
        public async Task<IActionResult> Del(long id)
        {
            bool res = await idNameService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除IdName失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除IdName成功" });
        }
        #endregion
    }
}