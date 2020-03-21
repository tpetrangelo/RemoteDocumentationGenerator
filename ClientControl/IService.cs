using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml;
using System.IO;
using System.Collections.Specialized;


namespace ServiceControl
{
    [ServiceContract]
    public interface IService
    {
        //Functions for Login
        [OperationContract]
        bool FindUsername(string username);

        [OperationContract]
        bool MatchPassword(string username, string password);

        //Functions for CreateLogin
        [OperationContract]
        void AddToXML(string username, string password);

        [OperationContract]
        void CreateRootHTML(string username);

        [OperationContract]
        void CreateProjectHTML(string project, string username);
        
        //Functions for PostLogin
        [OperationContract]
        bool AddProject(string projectName, string username);

        [OperationContract]
        bool UploadFile(string uploadFile, string project, string username, string projectName);

        [OperationContract]
        List<string> PopulateProjects(string user);

        [OperationContract]
        string GetFullDestinationPath(string project, string user);

        [OperationContract]
        void DocumentationGenerator(string projectPath, string user, string project);

        [OperationContract]
        string GetFilePath(string project, string user, string file);

        [OperationContract]
        void AddProjectToRoot(string project, string user);

        //Functions for EditWindow
        [OperationContract]
        void SaveFile(string file, StringCollection writeBack);

        
    }

    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader]
        public string fileName { get; set; }

        [MessageBodyMember]
        public Stream transferStream { get; set; }
    }
}
