﻿using System;
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
    [PermController("产品管理")]
    public class ProductController : Controller
    {
        private IProductService productService;
        private IProductImageService productImageService;
        private int pageSize = 10;
        public ProductController(IProductService productService, IProductImageService productImageService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex=1)
        {
            var res = await productService.GetModelListAsync(keyword,startTime,endTime,pageIndex,pageSize);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        public IActionResult List1()
        {
            return View();
        }
        [PermAction("添加产品")]
        public async Task<IActionResult> Add(string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description)
        {
            var res = await productService.AddAsync(name, price, inventory, saleNumber, putaway, hotSale, description);
            if(res<=0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "添加产品失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "添加产品成功" });
        }

        [HttpPost]
        public async Task<IActionResult> GetImages(long id)
        {
            var res = await productImageService.GetModelListAsync(id);
            return Json(new AjaxResult { Status = 1, Data = res });
        }

        [PermAction("修改产品")]
        public async Task<IActionResult> Edit(long id, string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description)
        {
            var res = await productService.EditAsync(id, name, price, inventory, saleNumber, putaway, hotSale, description);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "编辑产品失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "编辑产品成功" });
        }

        [PermAction("删除产品")]
        public async Task<IActionResult> Del(long id)
        {
            var res = await productService.DelAsync(id);
            if (!res)
            {
                return Json(new AjaxResult { Status = 0, Msg = "删除产品失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "删除产品成功" });
        }

        #region 富文本编辑器上传图片
        //上传到本地服务器
        [HttpPost]
        public async Task<IActionResult> UpContentImage(IFormFile imgFile)
        {
            return Json(new { errno = "0", data = await ImageHelper.SaveAsync(imgFile)});
        }

        [HttpPost]
        public async Task<IActionResult> UpImages(long id, string[] imgFiles)
        {
            if (imgFiles.Count() <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "请选择上传的图片" });
            }
            List<string> lists = new List<string>();
            foreach (string imgFile in imgFiles)
            {
                if (imgFile.Contains("upload/"))
                {
                    lists.Add(imgFile);
                }
                else
                {
                    var res = await ImageHelper.Base64SaveAsync(imgFile);
                    if (!res.Key)
                    {
                        return Json(new AjaxResult { Status = 0, Msg = res.Value });
                    }
                    lists.Add(res.Value);
                }
            }
            List<string> imgUrls = lists.Distinct().ToList();
            long resid = await productImageService.AddAsync(id, imgUrls);
            if (resid <= 0)
            {
                return Json(new AjaxResult { Status = 0, Msg = "上传失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "上传成功" });
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