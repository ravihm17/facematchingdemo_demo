using System.IO;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Linq;
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
using System.Threading;

namespace facematchingdemo.Controllers
{
    public class HomeController : Controller
    {

        // private string subscriptionKeyValue = "0837d919cf6c440f927dcf5586bca2d8";
        //private string subscriptionKeyValue = "b5b2511d0f994cd89ffad2d7e0b8658c";
        //private string subscriptionKeyValue = "9fb432d6c1c045fab4c5bafe4c036423";
        // private string subscriptionKeyValue = "a237042a81e74bd9be629a03ee821323";        
        //private string subscriptionKeyValue = "b68b237b109c4eebab3bfa49817d35f4";
        //private string subscriptionKeyValue = "c95f432f948048b1ad4bde8b80b6c884";
        // private string subscriptionKeyValue = "81f51596ab4c473bad2ab5b624595787";
        //private string subscriptionKeyValue = "a237042a81e74bd9be629a03ee821323";
        // private string subscriptionKeyValue = "32a37ae0eb674bdf8c2db8222026c472";
        //private string subscriptionKeyValue = "05eadb7b347c47f9bda966432ae7587d";

       // private string subscriptionKeyValue = "1235624638c943148c82ca5c79ccbbef";
        //private string subscriptionKeyValue = "5ca82be538964c9382bdabc3b741cf96";
        //private string subscriptionKeyValue = "f1b4d7db39044dd68ebfff1003837c40";

        //private string subscriptionKeyValue = "3d03fd940bf943809bcc3be2a87bd1ae";
       // private string subscriptionKeyValue = "98cb4323e7a14a9e996d69190ae8b4c4";
      //private string subscriptionKeyValue = "9fcf0aec911642b5b792ce4dd1534d13";
        //private string subscriptionKeyValue = "f33da4f37ae2463892fb4dd4c0de6100";
        private string subscriptionKeyValue = "de02804b7c7b4422bef3aba88d8d2c39";

        public string _selectedFile; public string _faceListName = string.Empty;
        public int MatchedImgcount = 0;
        private string  Errormsg = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChatBot()
        {
            //ViewBag.Message = "Your application description page.";


            return View();
        }


        public ActionResult Error(string Errormsg)
        {

            ViewBag["Errormsg"] = Errormsg;
            return View();
        }

        
        public async Task<string> Get()

        {    //string webChatSecret = ConfigurationManager.AppSettings["w-bXFaeMZVw.cwA.mxc.xLcrhYC7j5UwajCEXai3LhcH-gSDvbdGGNyE-ZyzVFU"];

            string webChatSecret = "0uN6G1TaHW0.cwA.GhU.COVV1KHFZ9PjHckFXgZ70thpKiTGEeceIJ6fEX8p4AM";

            //string webChatSecret = ConfigurationManager.AppSettings[WebChatSecret];



            var request = new HttpRequestMessage(HttpMethod.Get, "https://webchat.botframework.com/api/tokens");

            request.Headers.Add("Authorization", "BOTCONNECTOR " + webChatSecret);



            HttpResponseMessage response = await new HttpClient().SendAsync(request);

            string token = await response.Content.ReadAsStringAsync();

            token = token.Replace("\"", "");



            // return $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/PigLatinBotJoeMayo?t={token}'></iframe>";

            // string htmlValue = $"<iframe width='400px' height='400px' src='https://webchat.botframework.com/embed/ComplianceBot1?t={token}'></iframe>";

            string htmlValue = $"<iframe  align='center' width='600px' height='500px' src = 'https://webchat.botframework.com/embed/policytestbotservice?t={token}'></iframe>";

            return htmlValue;

        }


        public void Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            var path = Server.MapPath("~/test.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
              
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


       
        public async Task<string> FindSimilarImages()
        {

            //Creation of facelist and sending all the images(the images to which we are going to do match) to the facelist

            _faceListName = Guid.NewGuid().ToString(); // Generating a unique group-id for the entire images 
            var faceServiceClients = new FaceServiceClient(subscriptionKeyValue); //calling the Face API using subscription key 
            try
            {
                await faceServiceClients.CreateFaceListAsync(_faceListName, _faceListName, "face_Images"); //Calling the API service'CreateFaceListAsync' to create a facelist with id/name as  _faceListName.
            }

            catch (FaceAPIException ex)
            {

                Errormsg = ex.ErrorMessage;
                return RedirectToAction("Error", "Home", new { Errormsg = Errormsg }).ToString();

             }

            DirectoryInfo DirInfo = new DirectoryInfo(@"C:\Image");

            Dictionary<string, string> DictionaryListofPersistanceIDAndImagePath = new Dictionary<string, string>();//Dictionary entry for storing the persistance id returned for each image from the Face API service

            try
            {
                foreach (var file in DirInfo.GetFiles("*.jpg"))
                {
                    string imgPath = @"C:\Image\" + file.ToString();
                    FileStream fStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                    var faces = await faceServiceClients.AddFaceToFaceListAsync(_faceListName, fStream); //Adding of each image content to the created facelist in the Face API using the service 'AddFaceToFaceListAsync'
                    DictionaryListofPersistanceIDAndImagePath.Add(faces.PersistedFaceId.ToString(), imgPath); //Storing the PersistedFaceId of the image returned by the Face API service and image path in dictionary

                }
            }
            //End
            catch (FaceAPIException ex)
            {
                ViewData["ExceptionMsg"] = ex.ErrorMessage;

            }


            // Sending  and matching the captured image with the images contained in the facelist

            //  string CapturedImgName = Server.MapPath("~/Image/CapturedImg.jpg");
            string CapturedImgName = Server.MapPath("~/test.jpg");

            string[] MatchedImgpath; //int MatchedImgcount = 0;

            using (var fileStream = System.IO.File.OpenRead(CapturedImgName))
            {
                var faceServiceClient = new FaceServiceClient(subscriptionKeyValue);
                var faces = await faceServiceClient.DetectAsync(fileStream); //Calling the Face API 'DetectAsync' to detect the captured image by sending the content of the captured image
                                                                             // After call it will return a faceid to the captured image 
                foreach (var f in faces)
                {
                    var faceId = f.FaceId; //Retrive the face id of the captured image
                    const int requestCandidatesCount = 20; // The number of the more confidece images to be rturned.Most matched image have more confidence value.
                                                           //confidence value is assigned by the Face API service based on the match.
                    try
                    {
                        var result = await faceServiceClient.FindSimilarAsync(faceId, _faceListName, requestCandidatesCount); // Matching the captured image with images by sending  faceId and _faceListName to the Face API 'FindSimilarAsync'
                                                                                                                              //The variable result contains the matched image's PersistedFaceId 
                        MatchedImgpath = new string[requestCandidatesCount];    //Declare an array with size 'requestCandidatesCount' to store the matched images path
                                                                                // int MatchedImgcount = 0;
                        foreach (var fr in result) //Loop through the PersistedFaceId of matched faces
                        {

                            if (fr.Confidence >= 0.8) //To check whether the confidence value of the matched image is >=0.8
                            {
                                if (DictionaryListofPersistanceIDAndImagePath.ContainsKey(fr.PersistedFaceId.ToString()))//To check whether the Persistance id is present in the dictionary
                                                                                                                         //if present retrive the curresponding image-path of that PersistedFaceId. 
                                {
                                    MatchedImgpath[MatchedImgcount] = DictionaryListofPersistanceIDAndImagePath[fr.PersistedFaceId.ToString()]; //Store the image-path in an array.This array contains all the matched image path which have confidence-value >=0.8
                                    MatchedImgcount = MatchedImgcount + 1;
                                }
                            }

                        }

                    }

                    catch (FaceAPIException ex)
                    {
                        ViewData["ExceptionMsg"] = ex.ErrorMessage;
                    }

                }
            }
            if(MatchedImgcount != 0)
            {
                return "found";
            }
            else
            {
                return "notfound";
            }
            //End
            

        }
    }
}