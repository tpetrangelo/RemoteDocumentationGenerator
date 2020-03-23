using System;
using System.Text;
namespace TestSpace
{
    public class TestClassString1
    {
        private string name;
        public TestClassString1(string nameInput)
        {
            name = nameInput;
        }
        public void setName(string nameSet)
        {
            name = nameSet;
        }
        public string getName()
        {
            return name;
        }
        public void printWords()
        {
            Console.Writeln("Here are words");
        }
    }
}
