using System;
using System.CodeDom;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mozart.Attributes;
using Mozart.Helpers;
using Mozart.Model;

namespace Mozart.Playground
{
    public interface IHuman
    {
        string Says();
    }

    [Export(true, InstanceRule.Singleton)]
    public interface IManager
    {
        string Name { get; }
    }

    [Export(true, InstanceRule.Singleton)]
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
        delegate T ObjectActivator<T>(params object[] args);
        private static ObjectActivator<T> GetActivator<T>
    (ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

            //compile it
            ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }
        static void Main(string[] args)
        {
            
            var ctor = typeof (Cat).GetExportableConstructor();

            ObjectActivator<Cat> createdActivator = GetActivator<Cat>(ctor);
            Cat instance = createdActivator(new DallasZoo(), new MrJones());
            Console.WriteLine(instance.Says());
            //var p = ctor.GetCtorParameters() as ParameterInfo[];
            ////var le = Expression.Lambda(
            ////                            Expression.New(ctor,p.Select(x => Expression.Parameter(x.GetType()))));

            ////var le = Expression.Lambda(Expression.New(ctor, p.Select(x => Expression.Parameter(x.GetType()))),p.Select(y => y));

            ////var lambda = Expression.Lambda(Expression.New(ctor, p.Select(x => Expression.Parameter(x.ParameterType, x.Name))));

            //var lambda = Expression.Lambda(Expression.New(ctor, Expression.Parameter(typeof(IZoo)), Expression.Parameter(typeof(IManager))));
            //var func = lambda.Compile();
            
            //var asas = func.DynamicInvoke(new DallasZoo(), new MrJones());
            //Console.WriteLine(asas.GetType());
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
