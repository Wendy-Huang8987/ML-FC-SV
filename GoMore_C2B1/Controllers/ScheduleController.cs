using GoMore_C2B1.DataContext;
using GoMore_C2B1.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GoMore_C2B1.Controllers
{
    public class ScheduleController : Controller
    {
        private ApplicationDbContext LinkFile_ = new ApplicationDbContext();
        private ApplicationDbContext ModelDB = new ApplicationDbContext();
        // GET: Schedule
        public ActionResult Display()
        {
            return View();
        }
        public ActionResult CheckModelExists() {

            return View();
        }
        public ActionResult DisplayProject()
        {
            if (TempData["ifcFile"] != null)
            {
                return View();
            }
            return RedirectToAction("ModelControl","Schedule");
        }
        [HttpPost]
        public ActionResult DisplayProject(string ProjectName, string ModelName)
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r_f = new StreamReader(filePath);
            string json_f = r_f.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json_f);
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();

            //byte[] file = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/MU-ALL.ifc"));
            //TempData["byte"] = file;
            DateTime start = DateTime.Now;
            string ifcfilename = ModelName + ".ifc";
            WebClient request = new WebClient();
            string url = "ftp://127.0.0.1/SV/" + ProjectName + "/" + ifcfilename;
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            try
            {
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                string hexString = BitConverter.ToString(newFileData).Replace("-", string.Empty).ToLower();
                base.TempData["ifcname"] = ifcfilename;
                base.TempData["ifcFile"] = hexString;
                base.TempData["ProjectName"] = ProjectName;
            }
            catch (WebException e)
            {
            }
            string jsonfilename = ModelName + ".json";
            string url2 = "ftp://127.0.0.1/SV/" + ProjectName + "/" + jsonfilename;
            try
            {
                byte[] newjsonData = request.DownloadData(url2);
                string fileString2 = System.Text.Encoding.UTF8.GetString(newjsonData);
                string hexString2 = BitConverter.ToString(newjsonData).Replace("-", string.Empty).ToLower();
                MemoryStream stream = new MemoryStream(newjsonData);
                using (StreamReader r = new StreamReader(stream))
                {
                    string json = r.ReadToEnd();
                    base.TempData["json"] = json;
                }
            }
            catch (WebException e2)
            {
            }
            if (base.Session["Account"] == null)
            {
                return base.RedirectToAction("Index", "Home");
            }
            if (base.Session["AccountType"].ToString() != "0")
            {
                DateTime end = DateTime.Now;
                TimeSpan timeSpan = end - start;
                base.TempData["time"] = "Download the IFC file from FTP took " + timeSpan.Milliseconds.ToString() + " ms!";
                return base.View();
            }
            return base.RedirectToActionPermanent("Display2Project", "Schedule");

        }
        public ActionResult Display2Project()
        {
            if (TempData["ifcFile"] != null)
            {
                return View();
            }
            return RedirectToAction("ModelControl", "Schedule");
        }
        public ActionResult DisplayModel()
        {
            if (TempData["ifcFile"] != null)
            {
                return View();
            }
            return RedirectToAction("ModelControl", "Schedule");
        }
        [HttpPost]
        public ActionResult DisplayModel(string ID)
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r_f = new StreamReader(filePath);
            string json_f = r_f.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json_f);
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();
            //byte[] file = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/MU-ALL.ifc"));
            //TempData["byte"] = file;
            List<LinkFileUrl> linkFile =  LinkFile_.LinkFiles.Where(x=>x.ID.Equals(ID)).ToList();
            if (linkFile != null)
            {
                foreach (LinkFileUrl fileUrl in linkFile)
                {
                    WebClient request = new WebClient();
                    string url = "ftp://127.0.0.1/SV/" + fileUrl.Url;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    try
                    {
                        byte[] newFileData = request.DownloadData(url);
                        string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                        string hexString = BitConverter.ToString(newFileData).Replace("-", string.Empty).ToLower();

                        TempData["ifcname"] = fileUrl.FileName;
                        TempData["ifcFile"] = hexString;
                        TempData["ProjectName"] = fileUrl.ProjectName;

                    }
                    catch (WebException e)
                    {
                        // Do something such as log error, but this is based on OP's original code
                        // so for now we do nothing.
                    }
                }
                if (Session["Account"] != null)
                {
                    if (Session["AccountType"].ToString() != "0")
                    {
                        return View();
                    }
                    return RedirectToActionPermanent("Display2Model", "Schedule");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Display2Model()
        {
            if (TempData["ifcFile"] != null)
            {
                return View();
            }
            return RedirectToAction("ModelControl", "Schedule");
        }

        public ActionResult ModelControl()
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r_f = new StreamReader(filePath);
            string json_f = r_f.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json_f);
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();
            if (Session["Account"] != null)
            {
                WebClient request = new WebClient();

                List<SVModelControl> ModelProject = LinkFile_.SVModelControl.ToList();
                foreach (SVModelControl sVModel in ModelProject)
                {
                    string url = "ftp://127.0.0.1/SV/" + sVModel.ImageUrl;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    try
                    {
                        byte[] newFileData = request.DownloadData(url);
                        sVModel.ImageUrl = Convert.ToBase64String(newFileData);

                    }
                    catch (WebException e)
                    {
                        // Do something such as log error, but this is based on OP's original code
                        // so for now we do nothing.
                    }
                }

                ViewBag.ModelProject = ModelProject;
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult GetAllProjectUnit(string ProjectName)
        {
            List<LinkFileUrl> linkFileUrls = LinkFile_.LinkFiles.Where(x => x.ProjectName.Equals(ProjectName) && x.FileFolder.Equals("Models")).ToList();
            return Json(new { success = linkFileUrls });
        }

        [HttpPost]
        public ActionResult UploadModel(string Author,string ProjectName,string ModelName,string Permit,string ImageName,string ModelType)
        {
            SVModelControl sVModel = new SVModelControl();
            Random generator = new Random();
            string ID = generator.Next(0, 1000000).ToString("D6")+"_"+ProjectName;
            sVModel.ID = ID;
            sVModel.Author = Author;
            sVModel.ProjectName = ProjectName;
            sVModel.ModelName = ModelName;
            sVModel.Permit = Permit;
            sVModel.ModelType = ModelType;
            sVModel.ImageUrl = ProjectName + "/" + ImageName;
            LinkFile_.SVModelControl.Add(sVModel);
            LinkFile_.SaveChanges();

            ModelNameDB modelName = new ModelNameDB();
            modelName.ID = ID;
            modelName.ProjectName = ProjectName;
            modelName.Author = Author;
            modelName.Permit = Permit;
            ModelDB.ModelNames.Add(modelName);
            ModelDB.SaveChanges();

            if (Request.Files.Count > 0)
            {
                string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
                StreamReader r_f = new StreamReader(filePath);
                string json_f = r_f.ReadToEnd();
                var data = (JObject)JsonConvert.DeserializeObject(json_f);
                string ftpUserName = data["ftpuser"].Value<string>();
                string ftpPassword = data["ftppass"].Value<string>();
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        UploadFileToFTP("ftp://127.0.0.1/", ftpUserName, ftpPassword, file,"SV/"+ ProjectName);
                        // Get the complete folder path and store the file inside it.  
                        //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        //file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult ScheduleUpload()
        {
            if (Session["Account"] != null)
            {
                List<ModelNameDB> modelNameDBs = LinkFile_.ModelNames.ToList();
                ViewBag.models = modelNameDBs;

                return View();
            }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult UnitFileUpload(List<HttpPostedFileBase> UnitFile,string UnitFileName, string Tag,string ProjectName,string FileFolder)
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r_f = new StreamReader(filePath);
            string json_f = r_f.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json_f);
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();
            Random generator = new Random();
            foreach (HttpPostedFileBase unitfile in UnitFile)
            {
                LinkFileUrl dB = new LinkFileUrl();
                
                string ID = generator.Next(0, 1000000).ToString("D6");
                string Url = ProjectName + "/" + FileFolder + "/" + unitfile.FileName;

                dB.ID = ID;
                dB.LinksUnit = UnitFileName;
                if (Tag != null)
                {
                    dB.Tag = Tag;
                }
                else {
                    dB.Tag = "NULL";
                }
                dB.FileFolder = FileFolder;
                dB.ProjectName = ProjectName;
                dB.Url = Url;
                dB.FileName = unitfile.FileName;
                LinkFile_.LinkFiles.Add(dB);
                LinkFile_.SaveChanges();

                UploadFileToFTP("ftp://127.0.0.1/", ftpUserName, ftpPassword, unitfile, "SV/" + ProjectName+"/"+ FileFolder);



            }
                Session["Lastvalue"] = ProjectName;
            return RedirectToAction("ScheduleUpload", "Schedule");
        }

        [HttpPost]
        public ActionResult SaveAllModelUnits(List<string> Units, string MainModel,string ProjectName)
        {
            if (Units != null) {
                foreach (string Unit in Units)
                {
                    LinkFileModel dB = new LinkFileModel();
                    string ID = Unit.Split(' ')[1] + "_" + MainModel.Split('.')[0] + "_" + ProjectName;

                    dB.ID = ID;
                    dB.UnitFileName = Unit.Split(' ')[0];
                    dB.MainModel = MainModel.Split('.')[0];
                    dB.ProjectName = ProjectName;
                    dB.Status = "未安装";
                    LinkFile_.LinkModel.Add(dB);
                    LinkFile_.SaveChanges();
                }
                return Json(new { success = "Save All Units Done" });
            }
            return Json(new { success = "Model no unit properties" });
        }
        [HttpPost]
        public ActionResult GetStatus(string UnitFileName, string ProjectName)
        {
            string UnitsNamme = UnitFileName.Split('.')[0];
            List<LinkFileModel> linkFiles = LinkFile_.LinkModel.Where(x => x.UnitFileName.Equals(UnitsNamme) && x.ProjectName.Equals(ProjectName)).ToList();
            
            if (linkFiles != null)
            {
                if (linkFiles.Count() != 0)
                {
                    string status = linkFiles[0].Status;
                    return Json(new { success = status });
                }
                return Json(new { success = "no set status" });
            }
            return Json(new { success = "get status error" });
        }
        [HttpPost]
        public ActionResult ChangeUnitStatus(string Units,string ProjectName,string Status) {

            List<LinkFileModel> linkFileModels = LinkFile_.LinkModel.Where(x => x.UnitFileName.Equals(Units) && x.ProjectName.Equals(ProjectName)).ToList();
            
            if (linkFileModels != null)
            {
                foreach (LinkFileModel linkFile in linkFileModels)
                {

                    var entity = LinkFile_.LinkModel.FirstOrDefault(x => x.ID.Equals(linkFile.ID));
                    entity.Status = Status;
                    LinkFile_.SaveChanges();

                    return Json(new { success = "Change Status Done!" });
                }
            }
            return Json(new { success = "Change Status Fail" });
        }

        [HttpPost]
        public ActionResult GetAllLinkModel(string MainModel,string ProjectName)
        {
            string MainModelName = MainModel.Split(new char[]
    {
        '.'
    })[0];
            List<LinkFileModel> linkFiles = (from x in this.LinkFile_.LinkModel
                                             where x.MainModel.Equals(MainModelName) && x.ProjectName.Equals(ProjectName)
                                             select x).ToList<LinkFileModel>();
            string htmlcode = "<ul ><li>" + MainModelName;
            List<string> statusNid = new List<string>();
            using (List<LinkFileModel>.Enumerator enumerator = linkFiles.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    LinkFileModel linkFileModel = enumerator.Current;
                    htmlcode = htmlcode + "<ul ><li>" + linkFileModel.UnitFileName + "<ul>";
                    List<LinkFileModel> linkUnitsFiles = (from x in this.LinkFile_.LinkModel
                                                          where x.MainModel.Equals(linkFileModel.UnitFileName) && x.ProjectName.Equals(ProjectName)
                                                          select x).ToList<LinkFileModel>();
                    statusNid.Add(linkFileModel.ID.Split(new char[]
                    {
                '_'
                    })[0] + "_" + linkFileModel.Status);
                    foreach (LinkFileModel Unit in linkUnitsFiles)
                    {
                        htmlcode = htmlcode + "<li>" + Unit.UnitFileName + "</li>";
                    }
                    htmlcode += "</ul></li></ul>";
                }
            }
            htmlcode += "</li></ul>";
            return base.Json(new
            {
                success = htmlcode,
                status = statusNid
            });
        }
        public void UploadFileToFTP(string ftp, string FTPadminName, string ftpPassword, HttpPostedFileBase file, string folder_Name)
        {

            byte[] fileBytes = null;
            string fileName = Path.GetFileName(file.FileName);
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                fileBytes = br.ReadBytes(file.ContentLength);
            }
            if (!DoesFtpDirectoryExist(folder_Name,ftp,FTPadminName,ftpPassword))
            {
                FtpWebRequest requestmkdir = (FtpWebRequest)WebRequest.Create(ftp + folder_Name);
                requestmkdir.Credentials = new NetworkCredential(FTPadminName, ftpPassword);
                requestmkdir.UsePassive = true;
                requestmkdir.UseBinary = true;
                requestmkdir.KeepAlive = false;
                requestmkdir.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse responsemkdir = (FtpWebResponse)requestmkdir.GetResponse();
                responsemkdir.Close();
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + folder_Name+ "/" + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(FTPadminName, ftpPassword);
            request.ContentLength = fileBytes.Length;
            request.UsePassive = true;
            request.UseBinary = true;   // or FALSE for ASCII files
            request.ServicePoint.ConnectionLimit = fileBytes.Length;
            request.EnableSsl = false;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileBytes, 0, fileBytes.Length);
                requestStream.Close();
            }
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }
        public bool DoesFtpDirectoryExist(string dirPath, string ftp, string FTPadmin, string ftppass)
        {
            string fullPath = ftp + dirPath + "/";

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(FTPadmin, ftppass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        [HttpPost]
        public ActionResult GetUnitModel(string Units,string ProjectName)
        {
            List<LinkFileUrl> linkFiles = LinkFile_.LinkFiles.Where(x => x.LinksUnit.Equals(Units) && x.ProjectName.Equals(ProjectName)).ToList();

            if (linkFiles != null)
            {
                DrawingsFile drawingsFile = new DrawingsFile();
                List<DrawingsFile> drawingsFileList = new List<DrawingsFile>();
                DocumentsFile documentsFile = new DocumentsFile();
                List<DocumentsFile> documentsFileList = new List<DocumentsFile>();
                ModelsFile modelsFile = new ModelsFile();
                List<ModelsFile> modelsFileList = new List<ModelsFile>();
                OthersFile othersFile = new OthersFile();
                List<OthersFile> othersFileList = new List<OthersFile>();
                foreach (LinkFileUrl fileModel in linkFiles)
                {
                    if (fileModel.FileFolder != null)
                    {
                        if (fileModel.FileFolder.Equals("Drawings"))
                        {
                            drawingsFile.ID = fileModel.ID;
                            drawingsFile.FileName = fileModel.FileName;
                            drawingsFile.Tag = fileModel.Tag;
                            drawingsFileList.Add(drawingsFile);
                        }
                        else if (fileModel.FileFolder.Equals("Documents"))
                        {
                            documentsFile.ID = fileModel.ID;
                            documentsFile.FileName = fileModel.FileName;
                            documentsFile.Tag = fileModel.Tag;
                            documentsFileList.Add(documentsFile);
                        }
                        else if (fileModel.FileFolder.Equals("Models"))
                        {
                            modelsFile.ID = fileModel.ID;
                            modelsFile.FileName = fileModel.FileName;
                            modelsFile.Tag = fileModel.Tag;
                            modelsFileList.Add(modelsFile);
                        }
                        else
                        {
                            othersFile.ID = fileModel.ID;
                            othersFile.FileName = fileModel.FileName;
                            othersFile.Tag = fileModel.Tag;
                            othersFileList.Add(othersFile);
                        }
                    }
                }

                var drawingsFileListStr = JsonConvert.SerializeObject(drawingsFileList);
                var documentsFileListStr = JsonConvert.SerializeObject(documentsFileList);
                var modelsFileListStr = JsonConvert.SerializeObject(modelsFileList);
                var othersFileListStr = JsonConvert.SerializeObject(othersFileList);

                var totalFiles = new List<totalFile>();
                totalFiles.Add(new totalFile { DrawingsFile = drawingsFileListStr , DocumentsFile = documentsFileListStr, ModelsFile= modelsFileListStr, OthersFile= othersFileListStr });

                var totalFilesListStr = JsonConvert.SerializeObject(totalFiles);

                    return Json(totalFilesListStr);

            }
            else
            {
                return Json(new { error = "error" });
            }
        }

        [HttpGet]
        public ActionResult DownloadSFile(string ID)
        {
            string filePath = Server.MapPath("~/App_Start/Json/FTPconfig.json");
            StreamReader r_f = new StreamReader(filePath);
            string json_f = r_f.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(json_f);
            string ftpUserName = data["ftpuser"].Value<string>();
            string ftpPassword = data["ftppass"].Value<string>();
            List<LinkFileUrl> linkFile = LinkFile_.LinkFiles.Where(x => x.ID.Equals(ID)).ToList();
            if (linkFile != null)
            {

                string url = "ftp://127.0.0.1/SV/" + linkFile[0].Url;

                WebClient request = new WebClient();
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                byte[] bytes = request.DownloadData(url);
                //file.FileName = fileUrl.FileName;
                //file.Bytes = bytes;
                //file.gvFiles = url;

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(linkFile[0].FileName, bytes);

                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("{0}.zip", linkFile[0].LinksUnit);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
                return View();         
            }
            return View();
        }

    }

}