using GoMore_C2B1.DataContext;
using GoMore_C2B1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using Newtonsoft.Json.Linq;

namespace GoMore_C2B1.Controllers
{
    public class FactoryController : Controller
    {
        private ApplicationDbContext FC = new ApplicationDbContext();
        // GET: Factory
        public ActionResult Upload()
        {
            if (Session["Account"] != null)
            {
                string Uploader = Session["Account"].ToString();
                List<FactoryModel> FCobj = FC.FCM.Where(x => x.Uploader.Equals(Uploader)).ToList();
                if (FCobj != null)
                {
                    return View(FCobj);
                }
                TempData["Msg"] = "Get uploaded history fail.";
                return View();

            }
            TempData["Msg"] = "Login first!";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Parameter()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Client(HttpPostedFileBase file,string option)
        {
            FactoryModel factoryModel = new FactoryModel();
            try
            {
                var jsonValues = new Dictionary<string, string>
                {
                    { "option", option }
                };
                StringContent sc = new StringContent(JsonConvert.SerializeObject(jsonValues), UnicodeEncoding.UTF8);

                HttpClient http = new HttpClient();
                string url = "http://192.168.0.221:987/demoapi/GASLabReceiveFile";
                MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY");

                var fileContent = new StreamContent(file.InputStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                mulContent.Add(fileContent, "file", file.FileName);
                mulContent.Add(sc, "input1");
                await http.PostAsync(url, mulContent);

                if (ModelState.IsValid)
                {
                    Random generator = new Random();
                    
                    factoryModel.ID = generator.Next(0, 1000000).ToString("D6");
                    factoryModel.FileName = file.FileName.Split('.')[0];
                    factoryModel.UploadTime = DateTime.UtcNow.ToString();
                    factoryModel.Uploader = Session["Account"].ToString();
                    factoryModel.FileType = file.FileName.Split('.')[1];
                    factoryModel.FileConvert2 = option;
                    FC.FCM.Add(factoryModel);
                    FC.SaveChanges();
                }
                TempData["Msg"] = "Post Successful";
            }
            catch (Exception)
            {
                TempData["Msg"] = "Post fail";
            }

            return RedirectToAction("Upload", "Factory");
        }

        [HttpPost]
        public ActionResult Download(string FileName, string FileType, string FileConvert2)
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json);
            string ftp = data["ftpserver"].Value<string>();
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();
            string ftpMainFolder = "FC/";

            string Foldername = FileName;
            List<Files> fileList = CreateZipFile(ftp, ftpUserName, ftpPassword, ftpMainFolder, Foldername);
            if (fileList.Count >= 1)
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    foreach (Files file in fileList)
                    {
                        zip.AddEntry(file.FileName, file.Bytes);
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("{0}.zip", FileName);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
            }
            else
            {
                TempData["error"] = "Download Fail,Please Contact us!";
            }

            return RedirectToAction("Upload", "Factory");
        }
        public List<string> CreateFTPList(string ftp, string ftpUserName, string ftpPassword, string ftpMainFolder, string Foldername)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpMainFolder + Foldername);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();
            reader.Close();
            response.Close();
            List<string> source = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return source;
        }
        public List<Files> CreateZipFile(string ftp, string ftpUserName, string ftpPassword, string ftpMainFolder, string Foldername)
        {
            List<Files> fileList = new List<Files>();
            foreach (string file in CreateFTPList(ftp, ftpUserName, ftpPassword, ftpMainFolder, Foldername))
            {
                if (file.Equals("Specification"))
                {
                    string ftpFolder_t = ftpMainFolder + Foldername + "/";
                    string Foldername_t = "Specification";
                    foreach (string file_t in CreateFTPList(ftp, ftpUserName, ftpPassword, ftpFolder_t, Foldername_t))
                    {

                        WebClient request_T = new WebClient();
                        string url = ftp + ftpFolder_t + Foldername_t + "/" + file_t;
                        request_T.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                        byte[] bytes = request_T.DownloadData(url);
                        fileList.Add(new Files() { FileName = file_t, Bytes = bytes, gvFiles = url });

                    }
                }
                else
                {

                    WebClient request = new WebClient();
                    string url_ = ftp + ftpMainFolder + Foldername + "/" + file;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    byte[] bytes_ = request.DownloadData(url_);
                    fileList.Add(new Files() { FileName = file, Bytes = bytes_, gvFiles = url_ });

                }
            }
            return fileList;
        }

        [HttpPost]
        public async Task RTLabReceiveFile(HttpPostedFileBase file)
        {

            Console.WriteLine("recevied your file,file name is :" + file.FileName);

            await SaveStream(file.InputStream, @"C:\FTP\FC\"+file.FileName.Split('.')[0], file.FileName);


        }

        static readonly CancellationTokenSource s_cts = new CancellationTokenSource();
        async Task SaveStream(Stream fileStream, string destinationFolder, string destinationFileName)
        {


            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string path = Path.Combine(destinationFolder, destinationFileName);

            using (FileStream outputFileStream = new FileStream(path, FileMode.CreateNew))
            {
                try
                {
                    await fileStream.CopyToAsync(outputFileStream);
                }
                catch (OperationCanceledException)
                {

                }
                finally
                {
                    s_cts.Dispose();
                }

            }
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static bool ServicePing(string address, int port)
        {
            bool serviceOnline;

            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(address, port);
                serviceOnline = true;
            }
            catch (Exception)
            {
                serviceOnline = false;
            }

            return serviceOnline;
        }

    }
}
