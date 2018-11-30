using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                string[] paths = { path };
                return paths;
            }
        }

        public static bool Base64Save(string base64File,out string res)
        {            
            if(string.IsNullOrEmpty(base64File))
            {
                res = "请选择图片文件";
                return false;
            }

            if (!base64File.Contains(";base64"))
            {
                res = base64File;
                return true;
            }
            string[] strs = base64File.Split(',');
            string[] formats = strs[0].Replace(";base64", "").Split(':');
            string img = strs[1];
            string format = formats[1];
            string[] imgFormats = { "image/png", "image/jpg", "image/jpeg", "image/bmp", "IMAGE/PNG", "IMAGE/JPG", "IMAGE/JPEG", "IMAGE/BMP" };

            if (!imgFormats.Contains(format))
            {
                res = "请选择正确的图片格式，支持png、jpg、jpeg、png格式";
                return false;
            }
            string ext = "." + format.Split('/')[1];
            byte[] imgBytes = null;
            try
            {
                imgBytes = Convert.FromBase64String(img);
            }
            catch (Exception ex)
            {
                res = "base64图片文件解码错误";
                return false;
            }

            string md5 = CommonHelper.GetMD5(imgBytes);
            string path = "/upload/" + DateTime.Now.ToString("yyyy") + "/" + md5 + ext;
            string fullPath = Directory.GetCurrentDirectory() + "/wwwroot" + path;
            new FileInfo(fullPath).Directory.Create();

            using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                stream.Write(imgBytes, 0, imgBytes.Length);
                res = path;
                return true;
            }            
        }

        public static async Task<KeyValuePair<bool, string>> Base64SaveAsync(string base64File)
        {
            bool flag=true;
            string res = "";
            if (string.IsNullOrEmpty(base64File))
            {
                res = "请选择图片文件";
                return new KeyValuePair<bool, string>(flag,res);
            }

            if (!base64File.Contains(";base64"))
            {
                res = base64File;
                return new KeyValuePair<bool, string>(flag, res);
            }
            string[] strs = base64File.Split(',');
            string[] formats = strs[0].Replace(";base64", "").Split(':');
            string img = strs[1];
            string format = formats[1];
            string[] imgFormats = { "image/png", "image/jpg", "image/jpeg", "image/bmp", "IMAGE/PNG", "IMAGE/JPG", "IMAGE/JPEG", "IMAGE/BMP" };

            if (!imgFormats.Contains(format))
            {
                res = "请选择正确的图片格式，支持png、jpg、jpeg、png格式";
                flag = false;
                return new KeyValuePair<bool, string>(flag, res);
            }
            string ext = "." + format.Split('/')[1];
            byte[] imgBytes = null;
            try
            {
                imgBytes = Convert.FromBase64String(img);
            }
            catch (Exception ex)
            {
                res = "base64图片文件解码错误";
                flag = false;
                return new KeyValuePair<bool, string>(flag, res);
            }

            string md5 = CommonHelper.GetMD5(imgBytes);
            string path = "/upload/" + DateTime.Now.ToString("yyyy") + "/" + md5 + ext;
            string fullPath = Directory.GetCurrentDirectory() + "/wwwroot" + path;
            new FileInfo(fullPath).Directory.Create();

            using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                await stream.WriteAsync(imgBytes, 0, imgBytes.Length);
                res = path;
                return new KeyValuePair<bool, string>(flag, res);
            }
        }
    }
}
