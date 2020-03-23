///////////////////////////////////////////////////////////////////////
// Service.cs - Implementation of IService.cs                        //
// ver 1.0                                                           //
// Language:    C#, 2020, .Net Framework 4.7                         //
// Platform:    Lenovo Thinkpad X1 Carbon, Win10 Pro                 //
// Application: Documentation Generator, Project #3, Winter 2020     //
// Author:      Tom Petrangelo, Syracuse University                  //
//              thpetran@syr.edu                                     //
//                                                                   //
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * -------------------
 * 
 * Service is used to implement all of the back-end work for the client.
 * It will handle searching for users, creating HTML pages, and creating a
 * service channel for communication
 * 
 * Required Files
 * ---------------
 * IService.cs
 * 
 * Notes
 * ------
 * 
 * Used Dr. Fawcett's FileStreaming solution to help with downloading and uploading files
 * along with creating a channel
*/


using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using CodeAnalysis; 
 
namespace ServiceControl
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class Service : IService
    {
        int BlockSize = 1024;
        byte[] block;
        public Service()
        {
          block = new byte[BlockSize];
        }

        //Finds usernames in the XML file
        public bool FindUsername(string username)
        {
            XmlDocument xmlDoc = LoadXML();

            XmlNodeList nodeList = xmlDoc.SelectNodes("/Users/User/Username");
            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode.InnerText == username)
                    return true;
            }
            return false;
        }
        //Matches the user-input password with the current XML file
        public bool MatchPassword(string username, string password)
        {
            XmlDocument xmlDoc = LoadXML();

            XmlNodeList nodeListUsername = xmlDoc.SelectNodes("/Users/User/Username");
            XmlNodeList nodeListPassword = xmlDoc.SelectNodes("/Users/User/Password");
            foreach (XmlNode xmlNodeUsername in nodeListUsername)
            {
                if (xmlNodeUsername.InnerText == username)
                {
                    if (xmlNodeUsername.NextSibling.InnerText == password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Loads up the username and password XML file for querying
        private XmlDocument LoadXML()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../UsernamesPasswords.xml");
            return xmlDocument;
        }

        //Adds a user, password, and root to the XML file
        public void AddToXML(string username, string password)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNode user = xmlDocument.CreateElement("User");
            XmlNode _username = xmlDocument.CreateElement("Username");
            XmlNode pw = xmlDocument.CreateElement("Password");
            XmlNode root = xmlDocument.CreateElement("Root");
            XmlAttribute rootId = xmlDocument.CreateAttribute("id");
            _username.InnerText = username;
            pw.InnerText = password;
            rootId.Value = username;
            root.Attributes.SetNamedItem(rootId);
            user.AppendChild(_username);
            user.AppendChild(pw);
            user.AppendChild(root);
            xmlDocument.DocumentElement.AppendChild(user);
            xmlDocument.Save("../../UsernamesPasswords.xml");
            System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username);
            System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username + "/DownloadedFiles");
            System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username + "/Root");

        }

        //Adds a project to the XML file under the root id, it also creates the project directory in the Service->Repos folder
        public bool AddProject(string projectName, string username)
        {
            XmlDocument xmlDocument = LoadXML();

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Root");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlAttribute projectID = xmlDocument.CreateAttribute("id");
            projectID.Value = projectName;

            foreach (XmlNode rootUser in xmlNodeList)
            {
                if(rootUser.Attributes["id"].Value.ToString() == username)
                {
                    foreach (XmlNode proj in xmlProjectList)
                    {
                        if (proj.InnerText == projectName)
                        {
                            return false;
                        }
                    }
                    XmlNode project = xmlDocument.CreateElement("Project");
                    project.Attributes.SetNamedItem(projectID);
                    rootUser.AppendChild(project);
                    xmlDocument.Save("../../UsernamesPasswords.xml");
                    System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username + "/" + projectName);
                    System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username + "/" + projectName + "/html");
                    System.IO.Directory.CreateDirectory("../../../Service/Repos/" + username + "/" + projectName + "/source");
                }
            }
            return true;
        }

        //Iniitiates the uploading of a file to a directory for the user
        public bool UploadFile(string filePath, string projectPath, string username, string projectName)
        {
            string file = Path.GetFileName(filePath);
            using(var inputStream = new FileStream(filePath, FileMode.Open))
            {
                FileTransferMessage message = new FileTransferMessage();
                message.fileName = file;
                message.transferStream = inputStream;
                message.projectPath = projectPath;
                message.user = username;
                this.upLoadFile(message) ;
                
            }
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Root");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");
            foreach (XmlNode rootUser in xmlNodeList)
            {
                if (rootUser.Attributes["id"].Value.ToString() == username)
                {
                    foreach (XmlNode proj in xmlProjectList)
                    {
                        if(proj.Attributes["id"].Value.ToString() == projectName)
                        {
                            foreach (XmlNode fileN in xmlFileList)
                            {
                                if (fileN.InnerText == file)
                                {
                                    return false;
                                }
                            }
                            XmlNode fileX = xmlDocument.CreateElement("File");
                            fileX.InnerText = file;
                            proj.AppendChild(fileX);
                            xmlDocument.Save("../../UsernamesPasswords.xml");
                        }
                    }
                    
                }
            }
            return true;
        }

        //Populates all projects for drop-down for use by the user
        public List<string> PopulateProjects(string user)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            List<string> userProjects = new List<string>();

            userProjects.Clear();

            if (userProjects.Count != 0 && userProjects[0] == "-")
                userProjects.RemoveAt(0);

            foreach(XmlNode proj in xmlProjectList)
            {
                if(proj.ParentNode.Attributes["id"].Value.ToString() == user)
                {
                    userProjects.Add(proj.Attributes["id"].Value.ToString());
                }
                        
            }
            if(userProjects.Count == 0)
                userProjects.Add("-");
            return userProjects;
        }

        //Creates the communication between client and host
        public static ServiceHost CreateChannel(string url)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = 50000000;
            Uri address = new Uri(url);
            Type service = typeof(ServiceControl.Service);
            ServiceHost host = new ServiceHost(service, address);
            host.AddServiceEndpoint(typeof(IService), binding, address);
            return host;
        }

        //Returns the destination folder path for the project specified
        public string GetFullDestinationPath(string project, string user)
        {
            return "../../../Service/Repos/" + user + "/" + project + "/source";
        }

        //From a DLL, the passed in files are created in HTML format and placed into the project folder
        public void DocumentationGenerator(string projectPath, string user, string project)
        {
            FileInitiator.DocMain(projectPath, user, project);
        }

        //Returns the destination file path for the project specified

        public string GetFilePath(string project,string user, string file)
        {
            return "../../../Service/Repos/" + user + "/" + project + "/source/" + file;
        }

        //Populates all possible editable files by the for the user
        public List<string> PopulateEditFiles(string user)
        {
            XmlDocument xmlDocument = LoadXML();
           
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");
            List<string> editFiles = new List<string>();

            editFiles.Clear();

            if (editFiles.Count != 0 && editFiles[0].ToString() == "-")
                editFiles.RemoveAt(0);

            foreach (XmlNode file in xmlFileList)
            {
                if (file.ParentNode.ParentNode.Attributes["id"].Value.ToString() == user)
                {
                    editFiles.Add(file.InnerText.ToString());
                }

            }

            if (editFiles.Count == 0)
                editFiles.Add("-");
            return editFiles;
        }

        //Saves a file after editing
        public void SaveFile(string file, StringCollection writeBack)
        {
            string[] newFile = new string[writeBack.Count];
            writeBack.CopyTo(newFile, 0);
            System.IO.File.WriteAllLines(file, newFile);
        }

        //Writes to a specified file through FileStream, used to simulate and upload
        public void upLoadFile(FileTransferMessage msg)
        {
            string projectPath = msg.projectPath;
            string filename = msg.fileName;
            string rfilename = Path.Combine(projectPath + "/", filename);
            string userName = msg.user;
            if (!Directory.Exists(projectPath))
                Directory.CreateDirectory(projectPath);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = msg.transferStream.Read(block, 0, BlockSize);
                    if (bytesRead > 0)
                        outputStream.Write(block, 0, bytesRead);
                    else
                        break;
                }
            }
        }

        //Returns a list of all files, for use when trying to download or view a file
        public List<string> PopulateFiles()
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");
            List<string> allFiles = new List<string>();

            if (allFiles.Count != 0 && allFiles[0] == "-")
                allFiles.RemoveAt(0);

            foreach (XmlNode x in xmlNodeList)
            {
                foreach (XmlNode proj in xmlProjectList)
                {
                    foreach (XmlNode file in xmlFileList)
                    {
                        allFiles.Add(file.InnerText);
                    }
                }
            }

            if (allFiles.Count == 0)
                allFiles.Add("-");

            return allFiles;
        }

        //Returns a project based on the file given
        public string GetProject(string fileName)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");


            foreach (XmlNode x in xmlNodeList)
            {
                foreach (XmlNode proj in xmlProjectList)
                {
                    foreach (XmlNode file in xmlFileList)
                    {
                        if(file.InnerText == fileName)
                        {
                            return file.ParentNode.Attributes["id"].Value.ToString();
                        }
                        
                    }
                }
            }

            return "-";
        }

        //Returns the owner of a file based on the filen given
        public string GetUser(string fileName)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");

            foreach (XmlNode x in xmlNodeList)
            {
                foreach (XmlNode proj in xmlProjectList)
                {
                    foreach (XmlNode file in xmlFileList)
                    {
                        if (file.InnerText == fileName)
                        {
                            return file.ParentNode.ParentNode.Attributes["id"].Value.ToString();
                        }

                    }
                }
            }

            return "-";
        }

        //Initiates a file download based on the location of the file and the user to download to the file to
        public void DownloadFile(string fileLoc, string user)
        {
            string file = Path.GetFileName(fileLoc);
            string destinationPath = "../../../Service/Repos/" + user + "/DownloadedFiles/";
            Stream strm = this.downLoadFile(file, fileLoc);
            string rfilename = Path.Combine(destinationPath, file);
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = strm.Read(block, 0, BlockSize);
                    if (bytesRead > 0)
                        outputStream.Write(block, 0, bytesRead);
                    else
                        break;
                }
            }

        }

        //Creates an HTML file of the root user
        public void CreateRootHTML(string user)
        {
            string destinationPath = "../../../Service/Repos/" + user + "/Root/" + user + ".html";
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><link rel= \"stylesheet\" href=\"..\\..\\..\\..\\css\\Stylesheet.css\" type=\"text/css\"><title>" + user + "</title></head><body><h1 style=\"text-align:center\">" + user + "</h1>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destinationPath))
            {
                file.WriteLine(sb.ToString());
            }
        }

        //Creates an HTML file for a new project
        public void CreateProjectHTML(string project, string user)
        {
            string rootPath = "../../../" + user + "/Root/" + user + ".html";
            string destinationPath = "../../../Service/Repos/" + user + "/" + project + "/html/" + project + ".html";
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><link rel= \"stylesheet\" href=\"..\\..\\..\\..\\..\\css\\Stylesheet.css\" type=\"text/css\"><title>" + project + "</title></head><body><h1 style=\"text-align:center\">" + project + "</h1>");
            sb.Append("<a href =\"" + rootPath + "\">" + user + "</a></br>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destinationPath))
            {
                file.WriteLine(sb.ToString());
            }
        }

        //Adds the HTML link of a project to the users-root page
        public void AddProjectToRoot(string project, string user)
        {
            string destinationPath = "../../../Service/Repos/" + user + "/Root/" + user + ".html";
            string projectPath = "../../../Repos/" + user + "/" + project + "/html/" + project + ".html";
            string fileContent = File.ReadAllText(destinationPath);
            int bodyIndex = fileContent.IndexOf("</body>");
            string body = fileContent.Substring(0,bodyIndex);
            StringBuilder sb = new StringBuilder();
            sb.Append(body);
            sb.Append("<a href=\"" + projectPath + "\">" + project + "</a></br>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destinationPath))
            {
                file.WriteLine(sb.ToString());
            }
        }

        //Adds an HTML link of a file that has been generated to a project's page
        public void AddFileToProject(string file, string project, string user)
        {
            string rootPath = "../../../Service/Repos/" + user + "/" + project + "/html/" + project + ".html";
            string filePath = "../../../../../Service/Repos/" + user + "/" + project + "/html/" + file.Replace(".cs","") + ".html";
            string fileContent = File.ReadAllText(rootPath);
            int bodyIndex = fileContent.IndexOf("</body>");
            string body = fileContent.Substring(0, bodyIndex);
            StringBuilder sb = new StringBuilder();
            sb.Append(body);
            sb.Append("<a href=\"" + filePath + "\">" + file + "</a></br>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter fileN = new System.IO.StreamWriter(rootPath))
            {
                fileN.WriteLine(sb.ToString());
            }
        }

        //Returns a Stream when trying to download a file
        public Stream downLoadFile(string filename, string filePath)
        {
           
            FileStream outStream = null;
            if (File.Exists(filePath))
            {
                outStream = new FileStream(filePath, FileMode.Open);
            }
            else
                throw new Exception("open failed for \"" + filename + "\"");
            return outStream;
        }
    }
}
