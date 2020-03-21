//This is a comment
//This is also a test comment
/*More Comments*/
using System;
using System.Text;
namespace TestSpace
{
    public class Main
    {
        static void Main(string[] args)
        {
            //Here is another comment
            TestClassString1 t1 = new TestClassString1();
            t1.setName("Bob");    
            t1.printWords();
            TestClassInt2 t2 = new TestClassInt2();
            int inverse = t2.returnInverseSign(5);
            inverse = t2.square(inverse);
            TestClassIntString3 t3 = new TestClassIntString3(t2.abs(inverse),t1.GetName());
            t3.printNameMultiple();
        }
    }
}

