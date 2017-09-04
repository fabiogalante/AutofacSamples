using System;
using Autofac;

namespace AutofacSamples
{

    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

  


    public class EmailLog : ILog
    {
        private const string AdminEmail =  "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {AdminEmail} : {message}");
        }
    }


    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }


    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward...");
        }
    }

  


    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<ConsoleLog>().As<ILog>().AsSelf();
            //builder.RegisterType<EmailLog>().As<ILog>().AsSelf();



            builder.RegisterType<ConsoleLog>().As<ILog>();
            

            //builder.RegisterType<EmailLog>()
            //    .As<ILog>();
               





            builder.RegisterType<Engine>();
            builder.RegisterType<Car>()
                //  .UsingConstructor(typeof(Engine)); or
                .UsingConstructor(typeof(Engine), typeof(ILog));



            //var log = new ConsoleLog();
            //var engine = new Engine(log);
            //var car = new Car(engine,log);

            IContainer container = builder.Build();

            //
           // var log = container.Resolve<ConsoleLog>();

           

            var car = container.Resolve<Car>();

            car.Go();
        }
    }
}
