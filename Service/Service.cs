using System.Xml;
using System.ServiceModel;
using System.Collections.Generic;
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
            _username.InnerText = username;
            pw.InnerText = password;
            user.AppendChild(_username);
            user.AppendChild(pw);
            xmlDocument.DocumentElement.AppendChild(user);
            xmlDocument.Save("../../UsernamesPasswords.xml");

        }

        public bool AddProject(string projectName, string username)
        {
            XmlDocument xmlDocument = LoadXML();

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Project");


            foreach (XmlNode x in xmlNodeList)
            {
                if(x.InnerText == username)
                {
                    foreach (XmlNode proj in xmlProjectList)
                    {
                        if (proj.InnerText == projectName)
                        {
                            return false;
                        }
                    }
                    XmlNode project = xmlDocument.CreateElement("Project");
                    project.InnerText = projectName;
                    x.ParentNode.AppendChild(project);
                    xmlDocument.Save("../../UsernamesPasswords.xml");
                }
            }
            return true;
        }

        public void UploadFile()
        {

        }

        public List<string> PopulateProjects(string user)
        {
            XmlDocument xmlDocument = LoadXML();
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Users/User/Username");
            XmlNodeList xmlProjectList = xmlDocument.SelectNodes("/Users/User/Project");
            List<string> userProjects = new List<string>();
            
            foreach (XmlNode x in xmlNodeList)
            {
                if (x.InnerText == user)
                {
                    foreach(XmlNode proj in xmlProjectList)
                    {
                        userProjects.Add(proj.InnerText);
                    }
                    return userProjects;
                }
            }
            userProjects.Add("-");
            return userProjects;
        }


    }
}
