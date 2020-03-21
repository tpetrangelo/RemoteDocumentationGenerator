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

//using CodeAnalysis;


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

        private XmlDocument LoadXML()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../UsernamesPasswords.xml");
            return xmlDocument;
        }

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

        public bool UploadFile(string filePath, string projectPath, string username, string projectName)
        {
            string file = Path.GetFileName(filePath);
            using(var inputStream = new FileStream(filePath, FileMode.Open))
            {
                FileTransferMessage message = new FileTransferMessage();
                message.fileName = file;
                message.transferStream = inputStream;
                message.projectPath = projectPath;
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

        public List<string> PopulateProjects(string user)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            List<string> userProjects = new List<string>();

            if (userProjects.Count != 0 && userProjects[0] == "-")
                userProjects.RemoveAt(0);

            foreach (XmlNode x in xmlNodeList)
            {
                if (x.InnerText == user)
                {
                    foreach(XmlNode proj in xmlProjectList)
                    {
                        userProjects.Add(proj.Attributes["id"].Value.ToString());
                    }
                    return userProjects;
                }
            }
            userProjects.Add("-");
            return userProjects;
        }

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

        public string GetFullDestinationPath(string project, string user)
        {
            return "../../../Service/Repos/" + user + "/" + project + "/source";
        }

        public void DocumentationGenerator(string project)
        {
        }

        public string GetFilePath(string project,string user, string file)
        {
            return "../../../Service/Repos/" + user + "/" + project + "/source/" + file;
        }

        public List<string> populateEditFiles(string user)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Root/Project");
            XmlNodeList xmlFileList = xmlDocument.SelectNodes("/Users/User/Root/Project/File");
            List<string> editFiles = new List<string>();

            if (editFiles.Count != 0 && editFiles[0].ToString() == "-")
                editFiles.RemoveAt(0);

            foreach (XmlNode x in xmlNodeList)
            {
               
                if (x.InnerText == user)
                {
                    foreach (XmlNode proj in xmlProjectList)
                    {
                        foreach(XmlNode file in xmlFileList)
                        {
                            editFiles.Add(file.InnerText);
                        }
                    }
                }
            }
            if(editFiles.Count == 0)
                editFiles.Add("-");

            return editFiles;

        }

        public void SaveFile(string file, StringCollection writeBack)
        {
            string[] newFile = new string[writeBack.Count];
            writeBack.CopyTo(newFile, 0);
            System.IO.File.WriteAllLines(file, newFile);
        }

        public void upLoadFile(FileTransferMessage msg)
        {
            string projectPath = msg.projectPath;
            string filename = msg.fileName;
            string rfilename = Path.Combine(projectPath + "/", filename);
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
            
            return allFiles;
        }

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

        public void DownloadFile(string fileLoc, string user)
        {
            string file = Path.GetFileName(fileLoc);
            string destinationPath = "../../../Service/Repos/" + user + "/DownloadedFiles/" + file ;
            System.IO.File.Copy(fileLoc, destinationPath, true);
        }

        public void CreateRootHTML(string user)
        {
            string destinationPath = "../../../Service/Repos/" + user + "/Root/" + user + ".html";
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><title>" + user + "</title></head><body><h1 style=\"text-align:center\">" + user + "</h1>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destinationPath))
            {
                file.WriteLine(sb.ToString());
            }
        }

        public void CreateProjectHTML(string project, string user)
        {
            string rootPath = "../../../" + user + "/Root/" + user + ".html";
            string destinationPath = "../../../Service/Repos/" + user + "/" + project + "/html/" + project + ".html";
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><title>" + project + "</title></head><body><h1 style=\"text-align:center\">" + project + "</h1>");
            sb.Append("<a href =\"" + rootPath + "\">" + user + "</a>");
            sb.Append("</body></html>");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destinationPath))
            {
                file.WriteLine(sb.ToString());
            }
        }

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
    }
}
