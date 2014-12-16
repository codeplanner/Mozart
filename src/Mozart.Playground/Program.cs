using System;
using Mozart.Attributes;
using Mozart.Helpers;
using Mozart.Model;

namespace Mozart.Playground
{
    public interface IHuman
    {
        string Says();
    }

    [Export(typeof(IManager), true, InstanceRule.Singleton)]
    public interface IManager
    {
        string Name { get; }
    }

    [Export(typeof(IZoo), true, InstanceRule.Singleton)]
    public interface IZoo
    {
        string Name { get; }
    }

    [Export(typeof(IZoo), false, InstanceRule.Singleton)]
    public class BostonZoo : IZoo
    {
        public string Name { get; private set; }

        public BostonZoo()
        {
            Name = "Babaam";
        }
    }

    //[Export(typeof(IManager), false, InstanceRule.Singleton)]
    public class DallasZoo : IZoo
    {
        public string Name { get; private set; }

        public DallasZoo()
        {
            Name = "Babaam";
        }
    }

    public class MrJones : IManager
    {
        public string Name { get { return "Glen Jones"; } }
    }


    [Export]
    public interface IAnimal
    {
        string Says();
    }

    [NoExport]
    public class Hippo : IAnimal
    {
        public string Says()
        {
            return "Nothing...";
        }
    }

    //[Export(typeof(IAnimal))]
    public class Lion : IAnimal
    {
        public string Says()
        {
            return "Grrr";
        }
    }

    //[Export(typeof(IAnimal))]
    public class Dog : IAnimal
    {
        public string Says()
        {
            return "Voff";
        }
    }

    [Export(typeof(IAnimal))]
    public class Cat : IAnimal
    {
        public IZoo Zoo { get; private set; }
        public IManager Manager { get; private set; }
        public string Says()
        {
            return "Mjau";
        }

        public Cat() { }
        public Cat(IZoo zoo, IManager manager)
        {
            Zoo = zoo;
            Manager = manager;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {            
            //Export<IA>.Add(new A());

            //var exp =  Export<IA>.GetAll();
            //Console.WriteLine(exp.Count);
            //Console.WriteLine(Export<IA>.Exports.Count);

            //Mozart.Compose.Init();

            //var animals = Mozart.Compose<IZoo>.Get();
            //Console.WriteLine(animals.Name);


            //Compose.ExportedInterfaceFactory.Clear();
            
            Compose.Init();
            //Act            
            var obj = ReflectionHelpers.GetInstance<Cat>();

            // Assert
            Console.WriteLine(obj.Says());

            Console.ReadLine();
        }
    }
}
