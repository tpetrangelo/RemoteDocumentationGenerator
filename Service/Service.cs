using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Specialized;

//using CodeAnalysis;


namespace ServiceControl
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class Service : IService
    {
        public Service()
        {
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
            string fileName = Path.GetFileName(filePath);
            System.IO.File.Copy(filePath, projectPath + "/" + fileName, true);
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
                                if (fileN.InnerText == fileName)
                                {
                                    return false;
                                }
                            }
                            XmlNode file = xmlDocument.CreateElement("File");
                            file.InnerText = fileName;
                            proj.AppendChild(file);
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
            Uri address = new Uri(url);
            Type service = typeof(Service);
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

        }
    }
}
