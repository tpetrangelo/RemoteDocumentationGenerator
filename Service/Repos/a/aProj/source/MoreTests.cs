using System;
using System.Text;
using System.IO;
namespace TestSpace
{
    public class MoreTests
    {
        public MoreTests()
        {
            TestClassString1 t1 = new TestClassString1("Tom");
            t1.printWords();
            t1.setName("George");
            TestClassIntString3 = new TestClassIntString3(55, t1.getName());
        }
        public static void addFilesandScope(string fileName,List<TestSpace.Elem> fileElements)
        {
           List<string> namesOfScopes = new List<string>();
           foreach(CodeAnalysis.Elem className in fileElements)
           {
               if(className.type != "namespace")
               {
                    if (className.name.Length == 1)
                    {
                        if (Char.IsSymbol(className.name.ToCharArray()[className.name.Length - 1]) == false)
                        {
                            namesOfScopes.Add(className.name);
                        }
                    }
                    else
                    {
                        namesOfScopes.Add(className.name);
                    }
               }              
            }
            filesAndScopeDictionary.Add(fileName, namesOfScopes);
        }
    }
}
