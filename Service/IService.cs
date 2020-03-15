using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml;

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

        //Functions for PostLogin
        [OperationContract]
        bool AddProject(string projectName, string username);

        [OperationContract]
        void UploadFile(string uploadFile, string project);

        [OperationContract]
        List<string> PopulateProjects(string user);

        [OperationContract]
        string GetFullDestinationPath(string project, string user);

        [OperationContract]
        void DocumentationGenerator();

    }
}
