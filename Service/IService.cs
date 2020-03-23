///////////////////////////////////////////////////////////////////////
// IService.cs - Interface for Service.cs                            //
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
 * IService is used as the interface for Service.cs. This file contains
 * the service contract and operation contracts for the service.
 * It also hold the message contract information for 
 * upload and download
*/


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
        List<string> PopulateEditFiles(string user);

        [OperationContract]
        string GetFullDestinationPath(string project, string user);

        [OperationContract]
        void DocumentationGenerator(string projectPath, string user, string project);

        [OperationContract]
        string GetFilePath(string project, string user, string file);

        [OperationContract]
        void AddProjectToRoot(string project, string user);

        [OperationContract]
        void AddFileToProject(string file, string project, string user);

        [OperationContract]
        Stream downLoadFile(string filename, string filePath);


        //Functions for EditWindow
        [OperationContract]
        void SaveFile(string file, StringCollection writeBack);



    }

    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader]
        public string fileName { get; set; }

        [MessageHeader]
        public string user { get; set; }

        [MessageHeader]
        public string projectPath { get; set; }

        [MessageBodyMember]
        public Stream transferStream { get; set; }


    }
}
