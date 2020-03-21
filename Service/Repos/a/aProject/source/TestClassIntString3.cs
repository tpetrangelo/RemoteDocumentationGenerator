//
// This is a comment         
//
using System;
using System.Text;
namespace TestSpace
{
    public class TestClassIntString3
    {
        private int number;
        private string name;
        public TestClassString3(int num, string nameString)
        {
            number = num;
            name = nameString;
        }
        public void printNameMultiple()
        {
            for(int i = 0; i < number; i++)
            {
                Console.Writeln(name);
            }
        }
    }
}
