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

        //Function for PostLogin


    }
}
