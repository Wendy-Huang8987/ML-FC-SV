using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Npgsql;
using System.Data;
using System.Net;

namespace GoMore_C2B1.Models
{
    public class LibraryFiles
    {
        public string FileName { get; set; }
        public byte[] Bytes { get; set; }
        public string gvFiles { get; set; }

    }
    public class FileUploadModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public HttpPostedFileBase[] Specification { get; set; }
        public HttpPostedFileBase[] Object_M { get; set; }
        public HttpPostedFileBase img { get; set; }
    }
    public class Objectmodel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string OrganizationOfAuthor { get; set; }
        public string Owner { get; set; }
        public string ObjectName { get; set; }
        public string ObjectImage { get; set; }
        public string ObjectCategory { get; set; }
        public string ObjectIntroduction { get; set; }
        public string ReleaseDate { get; set; }
        public string VersionNumber { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }
        public string InstallSpecification { get; set; }
        public string TechnicalSpecification { get; set; }
        public string ProductMaterialStructure { get; set; }
        public string Country { get; set; }
        public string Unit { get; set; }

    }

    public class Object_f
    {

        public bool DoesFtpDirectoryExist(string dirPath, string ftp, string ftpuser, string ftppass)
        {
            string fullPath = ftp + dirPath + "/";

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpuser, ftppass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        public List<Objectmodel> GetUploadedModels(string Account, string host, string port, string database, string Username, string password)
        {
            string DBconfig = "Host = " + host + " ;Port =" + port + " ;Database =" + database + " ;Username = " + Username + " ; Password =" + password + ";";
            List<Objectmodel> displayobj = new List<Objectmodel>();
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Object_List where Author ='" + Account + "'";
            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                var obj = new Objectmodel();
                obj.Id = Convert.ToInt32(reader["Id"]);
                obj.Author = reader["Author"].ToString();
                obj.OrganizationOfAuthor = reader["OrganizationOfAuthor"].ToString();
                obj.Owner = reader["Owner"].ToString();
                obj.ObjectName = reader["ObjectName"].ToString();
                obj.ObjectImage = reader["ObjectImage"].ToString();
                obj.ObjectCategory = reader["ObjectCategory"].ToString();
                obj.ObjectIntroduction = reader["ObjectIntroduction"].ToString();
                obj.ReleaseDate = reader["ReleaseDate"].ToString();
                obj.VersionNumber = reader["VersionNumber"].ToString();
                obj.Width = (float)Convert.ToDouble(reader["Width"]);
                obj.Height = (float)Convert.ToDouble(reader["Height"]);
                obj.Depth = (float)Convert.ToDouble(reader["Depth"]);
                obj.InstallSpecification = reader["InstallSpecification"].ToString();
                obj.TechnicalSpecification = reader["TechnicalSpecification"].ToString();
                obj.ProductMaterialStructure = reader["ProductMaterialStructure"].ToString();
                obj.Country = reader["Country"].ToString();
                obj.Unit = reader["Unit"].ToString();
                displayobj.Add(obj);

            }
            conn.Close();

            return displayobj;

        }
        public List<Objectmodel> GetObjectDetail(Objectmodel Object, string host, string port, string database, string Username, string password)
        {
            //Object.ReleaseDate = Object.ReleaseDate.Replace("/", "").Replace(":", "").Replace("_", "");
            string DBconfig = "Host = " + host + " ;Port =" + port + " ;Database =" + database + " ;Username = " + Username + " ; Password =" + password + ";";
            List<Objectmodel> displayobj = new List<Objectmodel>();
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Object_List where ObjectName ='" + Object.ObjectName + "' and ReleaseDate ='" + Object.ReleaseDate + "' and Author ='" + Object.Author + "'";
            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                var obj = new Objectmodel();
                obj.Id = Convert.ToInt32(reader["Id"]);
                obj.Author = reader["Author"].ToString();
                obj.OrganizationOfAuthor = reader["OrganizationOfAuthor"].ToString();
                obj.Owner = reader["Owner"].ToString();
                obj.ObjectName = reader["ObjectName"].ToString();
                obj.ObjectImage = reader["ObjectImage"].ToString();
                obj.ObjectCategory = reader["ObjectCategory"].ToString();
                obj.ObjectIntroduction = reader["ObjectIntroduction"].ToString();
                obj.ReleaseDate = reader["ReleaseDate"].ToString();
                obj.VersionNumber = reader["VersionNumber"].ToString();
                obj.Width = (float)Convert.ToDouble(reader["Width"]);
                obj.Height = (float)Convert.ToDouble(reader["Height"]);
                obj.Depth = (float)Convert.ToDouble(reader["Depth"]);
                obj.InstallSpecification = reader["InstallSpecification"].ToString();
                obj.TechnicalSpecification = reader["TechnicalSpecification"].ToString();
                obj.ProductMaterialStructure = reader["ProductMaterialStructure"].ToString();
                obj.Country = reader["Country"].ToString();
                obj.Unit = reader["Unit"].ToString();
                displayobj.Add(obj);

            }
            conn.Close();

            return displayobj;

        }
        public List<Objectmodel> Category(Objectmodel Object, string host, string port, string database, string Username, string password)
        {
            string DBconfig = "Host = " + host + " ;Port =" + port + " ;Database =" + database + " ;Username = " + Username + " ; Password =" + password + ";";
            List<Objectmodel> displayobj = new List<Objectmodel>();
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Object_List where ObjectCategory ='" + Object.ObjectCategory + "'";
            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                var obj = new Objectmodel();
                obj.Id = Convert.ToInt32(reader["Id"]);
                obj.Author = reader["Author"].ToString();
                obj.OrganizationOfAuthor = reader["OrganizationOfAuthor"].ToString();
                obj.Owner = reader["Owner"].ToString();
                obj.ObjectName = reader["ObjectName"].ToString();
                obj.ObjectImage = reader["ObjectImage"].ToString();
                obj.ObjectCategory = reader["ObjectCategory"].ToString();
                obj.ObjectIntroduction = reader["ObjectIntroduction"].ToString();
                obj.ReleaseDate = reader["ReleaseDate"].ToString();
                obj.VersionNumber = reader["VersionNumber"].ToString();
                obj.Width = (float)Convert.ToDouble(reader["Width"]);
                obj.Height = (float)Convert.ToDouble(reader["Height"]);
                obj.Depth = (float)Convert.ToDouble(reader["Depth"]);
                obj.InstallSpecification = reader["InstallSpecification"].ToString();
                obj.TechnicalSpecification = reader["TechnicalSpecification"].ToString();
                obj.ProductMaterialStructure = reader["ProductMaterialStructure"].ToString();
                obj.Country = reader["Country"].ToString();
                obj.Unit = reader["Unit"].ToString();
                displayobj.Add(obj);

            }
            conn.Close();

            return displayobj;

        }
        public void objectUploadDB(Objectmodel objectmodel, string host, string port, string database, string Username, string password)
        {
            if (objectmodel.ObjectIntroduction == null)
            {
                objectmodel.ObjectIntroduction = "No have introduction";
            }
            if (objectmodel.ProductMaterialStructure == null)
            {
                objectmodel.ProductMaterialStructure = "No have Structure";
            }

            objectmodel.TechnicalSpecification = objectmodel.ReleaseDate.Replace("/", "").Replace(":", "").Replace("_", "") + "_" + objectmodel.ObjectName + "_" + objectmodel.Country + "/Specification";
            string DBconfig = "Host = " + host + " ;Port =" + port + " ;Database =" + database + " ;Username = " + Username + " ; Password =" + password + ";";
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "INSERT INTO Object_List(Author,OrganizationOfAuthor,Owner,ObjectName,ObjectImage,ObjectCategory,ObjectIntroduction,ReleaseDate,VersionNumber,Width,Height,Depth,InstallSpecification,TechnicalSpecification,ProductMaterialStructure,Country,Unit) VALUES (:Author, :OrganizationOfAuthor, :Owner, :ObjectName,:ObjectImage,:ObjectCategory,:ObjectIntroduction,:ReleaseDate,:VersionNumber,:Width,:Height,:Depth,:InstallSpecification,:TechnicalSpecification,:ProductMaterialStructure,:Country,:Unit)";
            cmd.Parameters.Add(new NpgsqlParameter("Author", objectmodel.Author));
            cmd.Parameters.Add(new NpgsqlParameter("OrganizationOfAuthor", objectmodel.OrganizationOfAuthor));
            cmd.Parameters.Add(new NpgsqlParameter("Owner", objectmodel.Owner));
            cmd.Parameters.Add(new NpgsqlParameter("ObjectName", objectmodel.ObjectName));
            cmd.Parameters.Add(new NpgsqlParameter("ObjectImage", objectmodel.ObjectImage));
            cmd.Parameters.Add(new NpgsqlParameter("ObjectCategory", objectmodel.ObjectCategory));
            cmd.Parameters.Add(new NpgsqlParameter("ObjectIntroduction", objectmodel.ObjectIntroduction));
            cmd.Parameters.Add(new NpgsqlParameter("ReleaseDate", objectmodel.ReleaseDate));
            cmd.Parameters.Add(new NpgsqlParameter("VersionNumber", objectmodel.VersionNumber));
            cmd.Parameters.Add(new NpgsqlParameter("Width", Convert.ToDouble(objectmodel.Width)));
            cmd.Parameters.Add(new NpgsqlParameter("Height", Convert.ToDouble(objectmodel.Height)));
            cmd.Parameters.Add(new NpgsqlParameter("Depth", Convert.ToDouble(objectmodel.Depth)));
            cmd.Parameters.Add(new NpgsqlParameter("InstallSpecification", objectmodel.InstallSpecification));
            cmd.Parameters.Add(new NpgsqlParameter("TechnicalSpecification", objectmodel.TechnicalSpecification));
            cmd.Parameters.Add(new NpgsqlParameter("ProductMaterialStructure", objectmodel.ProductMaterialStructure));
            cmd.Parameters.Add(new NpgsqlParameter("Country", objectmodel.Country));
            cmd.Parameters.Add(new NpgsqlParameter("Unit", objectmodel.Unit));
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UploadFileToFTP(string ftp, string ftpUserName, string ftpPassword, HttpPostedFileBase file, string folder_Name)
        {

            byte[] fileBytes = null;
            string fileName = Path.GetFileName(file.FileName);
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                fileBytes = br.ReadBytes(file.ContentLength);
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + folder_Name + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
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


        public static byte[] BinaryReadToEnd(Stream stream)
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
        public List<LibraryFiles> CreateZipFile(string ftp, string ftpUserName, string ftpPassword, string ftpMainFolder, string Foldername)
        {
            List<LibraryFiles> fileList = new List<LibraryFiles>();
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
                        fileList.Add(new LibraryFiles() { FileName = file_t, Bytes = bytes });

                    }
                }
                else
                {

                    WebClient request = new WebClient();
                    string url_ = ftp + ftpMainFolder + Foldername + "/" + file;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    byte[] bytes_ = request.DownloadData(url_);
                    fileList.Add(new LibraryFiles() { FileName = file, Bytes = bytes_ });

                }
            }
            return fileList;
        }
        public List<LibraryFiles> FindSpec(string ftp, string ftpUserName, string ftpPassword, string ftpMainFolder, string Foldername)
        {
            List<LibraryFiles> fileList = new List<LibraryFiles>();
            foreach (string file in CreateFTPList(ftp, ftpUserName, ftpPassword, ftpMainFolder, Foldername))
            {
                WebClient request = new WebClient();
                string url_ = ftp + ftpMainFolder + Foldername;
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                fileList.Add(new LibraryFiles() { FileName = file });

            }
            return fileList;
        }
    }
}