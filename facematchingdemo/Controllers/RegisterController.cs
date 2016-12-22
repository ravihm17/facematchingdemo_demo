using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
//using Microsoft.ProjectOxford.Face.Contract;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Net.Http;
using System.Threading;

namespace facematchingdemo.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        //public ActionResult Register()
        //{
        //    return View();
        //}

        string userName = string.Empty;
        //[HttpPost]
        //public void SaveCapturedImage(string txtUserName)
        //{
        //    userName = txtUserName.ToString();
        //}
        //[HttpPost]
        public ActionResult Capture(string username)
        {
            // userName = txtUserName.ToString();
            //userName= fc["txtUserName"].ToString();
            // userName = Request["txtUserName"].ToString();
            //userName = Request.Form["txtUserName"].ToString();
            //userName = para1;
            //userName= String.Format("{0}", Request.Form["txtUserName"]);
            userName = username.ToString();
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();


            if (!Directory.Exists(@"C:/Image/"))
                    Directory.CreateDirectory(@"C:/Image/");
            // var path = "~/"+ userName +".jpg";       
            var path = "C:/Image/" + userName + ".jpg"; 
             //path = Server.MapPath(path);
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
            // ViewBag["path"]= userName+".jpg";

            // path.CopyTo(Server.MapPath(0,@"C:\Image\")"));
            // return Json("success");
            //DirectoryInfo DirInfo = new DirectoryInfo(@"C:\Image");
            return View();
           
        }
        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;

        }

        //public string SaveImages()
        //{


        //}

    }
}