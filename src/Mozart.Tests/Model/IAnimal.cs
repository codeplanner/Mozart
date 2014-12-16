using Mozart.Attributes;
using Mozart.Model;

namespace Mozart.Tests.Model
{
    [Export(typeof(ICar))]
    public interface ICar
    {
        string RegNr { get; set; }
    }

    public class Car : ICar
    {
        public string RegNr { get; set; }        
    }

    public interface IHuman
    {
        string Says();
    }

    [Export(typeof(IManager))]
    public interface IManager
    {
        ICar Car { get; set; }
        string Name { get; }
    }

    [Export(typeof(IZoo), true, InstanceRule.Singleton)]
    public interface IZoo
    {
        string Name { get; }
    }

    public class BostonZoo : IZoo
    {
        public string Name { get; private set; }

        public BostonZoo()
        {
            Name = "Babaam";
        }        
    }

    [Export(typeof(IZoo), false, InstanceRule.Singleton)]
    public class DallasZoo : IZoo
    {
        public string Name { get; private set; }

        public DallasZoo()
        {
            Name = "Babaam";
        }
    }

    [Export(typeof(IManager))]
    public class MrJones : IManager
    {
        public ICar Car { get; set; }
        public string Name { get { return "Glen Jones"; } }

        public MrJones(ICar car)
        {
            Car = car;
        }
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

    public class Lion : IAnimal
    {
        public string Says()
        {
            return "Grrr";
        }
    }


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
}
