//This file Can be modified
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Api;
using System.Collections.Generic;
using System.Text;

namespace BigTask2.Ui
{
    interface IForm
    {
        void Insert(string command);
        bool GetBoolValue(string name);
        string GetTextValue(string name);
        int GetNumericValue(string name);
    }

    interface IDisplay
    {
        void Print(IEnumerable<Route> routes);
    }

    interface ISystem
    {
        IForm Form { get; }
        IDisplay Display { get; }
        ISystem Visit(string name, IMachine machine, int n);
    }

    //Machines to get Values
    interface IMachine
    {
        string Handle(List<string> comands, string name);
        IMachine SetNextMachine(IMachine machine);
    }

    class XMLMachine: IMachine
    {
        private IMachine nextMachine;
        private string option;
        private string first;
        private string second;

        public XMLMachine(string option)
        {
            this.option = option;

            StringBuilder sb = new StringBuilder();

            sb.Append("<").Append(option).Append(">");
            this.first = sb.ToString();
            sb.Clear();

            sb.Append("</").Append(option).Append(">");
            this.second = sb.ToString();
        }

        public string Handle(List<string> comands, string name)
        {
            if (name == option)
            {
                foreach (string comand in comands)
                {
                    if (comand.Length >= first.Length + second.Length 
                        && comand.Substring(0, first.Length) == first 
                        && comand.Substring(comand.Length - second.Length, second.Length) == second)
                    {
                        string ret = comand.Substring(first.Length, comand.Length - first.Length - second.Length);
                        if (ret == null)
                            return string.Empty;
                        return ret;
                    }
                }
            }
            return nextMachine.Handle(comands, name);
        }

        public IMachine SetNextMachine(IMachine machine)
        {
            this.nextMachine = machine;
            return machine;
        }
    }

    class INIMachine: IMachine
    {
        private IMachine nextMachine;
        private string option;
        private string get;

        public INIMachine(string option)
        {
            this.option = option;

            StringBuilder sb = new StringBuilder();

            sb.Append(option).Append("=");
            this.get = sb.ToString();
        }

        public string Handle(List<string> comands, string name)
        {
            if (name == option)
            {
                foreach (string comand in comands)
                {
                    if (comand.Length >= get.Length && comand.Substring(0, get.Length) == get) 
                    {
                        string ret = comand.Substring(get.Length);
                        if (ret == null)
                            return string.Empty;
                        return ret;
                    }
                }
            }
            return nextMachine.Handle(comands, name);
        }

        public IMachine SetNextMachine(IMachine machine)
        {
            this.nextMachine = machine;
            return machine;
        }
    }

    //Form implementation
    class Form: IForm
    {
        private List<string> comands = new List<string>();
        private int n;
        IMachine machine;

        public Form(IMachine machine, int n)
        {
            this.machine = machine;
            this.n = n;
        }

        public void Insert(string comand)
        {
            if (comands.Count == n)
                comands = new List<string>();
            this.comands.Add(comand);
        }

        public bool GetBoolValue(string name)
        {
            string ret = this.machine.Handle(this.comands, name);
            if (ret == "False")
                return false;
            else
                return true;
        }

        public string GetTextValue(string name)
        {
            return this.machine.Handle(this.comands, name);
        }

        public int GetNumericValue(string name)
        {
            return int.Parse(this.machine.Handle(this.comands, name));
        }
    }

    //Display implementation
    class XMLDisplay : IDisplay
    {
        public void Print(IEnumerable<Route> routes)
        {
            if (routes == null)
            {
                System.Console.WriteLine("<>");
                return;
            }

            double cost = 0.0;
            double time = 0.0;
            bool start = true;
            foreach (var r in routes)
            {
                if (start == true)
                {
                    this.PrintCity(r.From);
                    start = false;
                }
                this.PrintRoute(r);
                this.PrintCity(r.To);
                cost += r.Cost;
                time += r.TravelTime;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<totalTime>").Append(time).Append("</totalTime>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();
            sb.Append("<totalCost>").Append(cost).Append("</totalCost>");
            System.Console.WriteLine(sb.ToString());
            System.Console.WriteLine();
        }

        private void PrintCity(City city)
        {
            StringBuilder sb = new StringBuilder();

            System.Console.WriteLine("<City/>");

            sb.Append("<Name>").Append(city.Name).Append("</Name>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("<Population>").Append(city.Population).Append("</Population>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("<HasRestaurant>").Append(city.HasRestaurant).Append("</HasRestaurant>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            System.Console.WriteLine();
        }

        private void PrintRoute(Route route)
        {
            StringBuilder sb = new StringBuilder();

            System.Console.WriteLine("<Route/>");

            sb.Append("<Vehicle>").Append(route.VehicleType).Append("</Vehicle>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("<Cost>").Append(route.Cost).Append("</Cost>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("<TravelTime>").Append(route.TravelTime).Append("</TravelTime>");
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            System.Console.WriteLine();
        }
    }

    class INIDisplay : IDisplay
    {
        public void Print(IEnumerable<Route> routes)
        {
            if (routes == null)
            {
                System.Console.WriteLine("=");
                return;
            }            

            double cost = 0.0;
            double time = 0.0;
            bool start = true;
            foreach (var r in routes)
            {
                if (start == true)
                {
                    this.PrintCity(r.From);
                    start = false;
                }
                this.PrintRoute(r);
                this.PrintCity(r.To);
                cost += r.Cost;
                time += r.TravelTime;
            }
            System.Console.WriteLine($"totalTime={time}");
            System.Console.WriteLine($"totalCost={cost}");
            System.Console.WriteLine();
        }

        private void PrintCity(City city)
        {
            StringBuilder sb = new StringBuilder();

            System.Console.WriteLine("=City=");

            sb.Append("Name=").Append(city.Name);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("Population=").Append(city.Population);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("HasRestaurant=").Append(city.HasRestaurant);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            System.Console.WriteLine();
        }

        private void PrintRoute(Route route)
        {
            StringBuilder sb = new StringBuilder();

            System.Console.WriteLine("=Route=");

            sb.Append("Vehicle=").Append(route.VehicleType);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("Cost=").Append(route.Cost);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("TravelTime=").Append(route.TravelTime);
            System.Console.WriteLine(sb.ToString());
            sb.Clear();

            System.Console.WriteLine();
        }
    }

    //System implementation
    class XMLSystem : ISystem
    {
        private string name = "XML";
        public IDisplay Display { get; }
        public IForm Form { get; }

        public XMLSystem(IMachine machine, int n)
        {
            this.Display = new XMLDisplay();
            this.Form = new Form(machine, n);
        }

        public XMLSystem() { }

        public ISystem Visit(string name, IMachine machine, int n)
        {
            if (this.name == name)
                return new XMLSystem(machine, n);
            return null;
        }
    }

    class INISystem: ISystem
    {
        private string name = "INI";
        public IDisplay Display { get; }
        public IForm Form { get; }

        public INISystem(IMachine machine, int n)
        {
            this.Display = new INIDisplay();
            this.Form = new Form(machine, n);
        }

        public INISystem() { }

        public ISystem Visit(string name, IMachine machine, int n)
        {
            if (this.name == name)
                return new INISystem(machine, n);
            return null;
        }
    }
}
