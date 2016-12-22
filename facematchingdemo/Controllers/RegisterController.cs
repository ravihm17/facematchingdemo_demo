using System.IO;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
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

        string userName = string.Empty;
       
        public ActionResult Capture(string username)
        {
            
            userName = username.ToString();
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            if (!Directory.Exists(@"C:/Image/"))
                    Directory.CreateDirectory(@"C:/Image/");
              
            var path = "C:/Image/" + userName + ".jpg"; 
             
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
            
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
    }
}