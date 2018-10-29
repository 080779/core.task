using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ImageHelper
    {
        public async static Task<string[]> SaveAsync(IFormFile formFile)
        {
            string md5 = CommonHelper.GetMD5(formFile.OpenReadStream());
            string ext = Path.GetExtension(formFile.FileName);
            string path = "/upload/" + DateTime.Now.ToString("yyyy") + "/" + md5 + ext;
            string fullPath = Directory.GetCurrentDirectory()+"/wwwroot" + path;
            new FileInfo(fullPath).Directory.Create();
            using (FileStream createFileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
               await formFile.CopyToAsync(createFileStream);
            }                
            string[] paths = { path };
            return paths;
        }
    }
}
