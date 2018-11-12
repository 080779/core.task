using Microsoft.AspNetCore.Http;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class QiniuHelper
    {
        private static readonly string accessKey = "2hrivlc6eBhOJdE4wd-n0oXlg_m6Bz5pG-PJW4lB";
        private static readonly string secretKey = "q9tYfpo4JkpqYQfW5FiY1okFHeuGT7ylMkcNND_U";
        private static readonly string bucket = "mycloud2"; //存储空间名
        private static readonly string domain = "http://pi2nj2ap1.bkt.clouddn.com"; //融合 CDN 测试域名()
        public static async Task<KeyValuePair<int,string>> UploadStreamAsync(IFormFile imgFile)
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);

            Qiniu.Common.Config.AutoZone(accessKey, bucket, true);
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy


            //用“年/月/日/文件内容md5.文件类型”做文件名
            string saveKey = "upload/" + DateTime.Now.ToString("yyyy") + "/" + CommonHelper.GetMD5(imgFile.OpenReadStream()) + Path.GetExtension(imgFile.FileName);

            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            putPolicy.Scope = bucket + ":" + saveKey;
            //putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();

            //HttpResult result = um.UploadData(uploadFileBytes, saveKey, token);
            HttpResult result = await um.UploadStreamAsync(imgFile.OpenReadStream(), saveKey, token);
            KeyValuePair<int, string> keyValue;
            if (result.Code==200)
            {
                //domain+"/"+saveKey:http://osvu57cx3.bkt.clouddn.com/upload/2018/63527842B5DBB69BEBF9D86CB110DF6E.jpg
                keyValue = new KeyValuePair<int, string>(result.Code, domain + "/" + saveKey);
                return keyValue;
            }
            else
            {
                keyValue = new KeyValuePair<int, string>(result.Code, result.Text);
                return keyValue;
            }
        }
    }
}
