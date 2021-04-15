//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Api;
using BigTask2.Data;
using BigTask2.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using BigTask2.Interfaces;
using BigTask2.Problems;
using BigTask2.Algorithms;

namespace BigTask2
{
	class Program
	{
        static IEnumerable<Route> ServeRequest(Request request)
        {
            (IGraphDatabase cars, IGraphDatabase trains) = MockData.InitDatabases();
            /*
			 *
			 * Add request handling here and return calculated route
			 *
			 */

            //Verifying data
            if (request.From == string.Empty || request.To == string.Empty || request.Filter.MinPopulation < 0 || request.Filter.AllowedVehicles.Count < 1)
            {
                return null;
            }

            //Verifying type of problem
            IRouteProblem problem = DetermineProblem(request);

            //Verifying AllowedVehicles
            IGraphDatabase database;
            if (request.Filter.AllowedVehicles.Contains(VehicleType.Car) && request.Filter.AllowedVehicles.Contains(VehicleType.Train))
            {
                database = new CombinedTwoIGraphDatabaseDecorator(cars, trains);
            }
            else if (request.Filter.AllowedVehicles.Contains(VehicleType.Car))
            {
                database = cars;
            }
            else
            {
                database = trains;
            }

            //Filtering
            if (request.Filter.RestaurantRequired)
                database = new RestaurantRequiredIGraphDatabaseDecorator(database);
            database = new MinPopulationIGraphDatabaseDecorator(database, request.Filter.MinPopulation);

            //Add database
            problem.Graph = database;

            ISolver[] solvers = { new BFSSolver(), new DFSSolver(), new DijkstraSolver() };

            problem.Algorithm = request.Solver;
            foreach (var s in solvers)
            {
                IEnumerable<Route> ret = problem.Accept(s);
                if (problem.Solved)
                    return ret;
            }

            return null;
		}

		static void Main(string[] args)
		{
            Console.WriteLine("---- Xml Interface ----");
            /*
			 * Create XML System Here
             * and execute prepared strings
			 */

            //Preparing machines
            XMLMachine from = new XMLMachine("from");
            XMLMachine to = new XMLMachine("to");
            XMLMachine train = new XMLMachine("train");
            XMLMachine car = new XMLMachine("car");
            XMLMachine minPopulation = new XMLMachine("minPopulation");
            XMLMachine restaurantRequired = new XMLMachine("restaurantRequired");
            XMLMachine solver = new XMLMachine("solver");
            XMLMachine problem = new XMLMachine("problem");

            //Creating a chain
            from.SetNextMachine(to).SetNextMachine(train).SetNextMachine(car).SetNextMachine(minPopulation).SetNextMachine(restaurantRequired).SetNextMachine(solver).SetNextMachine(problem);

            ISystem xmlSystem = CreateSystem("XML", from, 8);
            Execute(xmlSystem, "xml_input.txt");
            Console.WriteLine();

            Console.WriteLine("---- KeyValue Interface ----");
            /*
			 * Create INI System Here
             * and execute prepared strings
			 */

            //Preparing machines
            INIMachine from2 = new INIMachine("from");
            INIMachine to2 = new INIMachine("to");
            INIMachine train2 = new INIMachine("train");
            INIMachine car2 = new INIMachine("car");
            INIMachine minPopulation2 = new INIMachine("minPopulation");
            INIMachine restaurantRequired2 = new INIMachine("restaurantRequired");
            INIMachine solver2 = new INIMachine("solver");
            INIMachine problem2 = new INIMachine("problem");

            //Creating a chain
            from2.SetNextMachine(to2).SetNextMachine(train2).SetNextMachine(car2).SetNextMachine(minPopulation2).SetNextMachine(restaurantRequired2).SetNextMachine(solver2).SetNextMachine(problem2);

            ISystem keyValueSystem = CreateSystem("INI", from2, 8);
            Execute(keyValueSystem, "key_value_input.txt");
            Console.WriteLine();
        }

        /* Prepare method Create System here (add return, arguments and body)*/
        static ISystem CreateSystem(string name, IMachine machine, int n) 
        {
            ISystem[] systems = { new XMLSystem(), new INISystem() };
            foreach (var s in systems)
            {
                ISystem system = s.Visit(name, machine, n);
                if (system != null)
                    return system;
            }
            throw new Exception("Nieznany system");
        }

        static IRouteProblem DetermineProblem(Request request)
        {
            IRouteProblem[] problems = { new CostProblem(), new TimeProblem() };
            foreach (var p in problems)
            {
                IRouteProblem problem = p.Visit(request);
                if (problem != null)
                {
                    return problem;
                }
            }
            throw new Exception("Nieznany typ problemu");
        }

        static void Execute(ISystem system, string path)
        {
            IEnumerable<IEnumerable<string>> allInputs = ReadInputs(path);
            foreach (var inputs in allInputs)
            {
                foreach (string input in inputs)
                {
                    system.Form.Insert(input);
                }
                var request = RequestMapper.Map(system.Form);
                var result = ServeRequest(request);
                system.Display.Print(result);
                Console.WriteLine("==============================================================");
            }
        }

        private static IEnumerable<IEnumerable<string>> ReadInputs(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                List<List<string>> allInputs = new List<List<string>>();
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    List<string> inputs = new List<string>();
                    while (!string.IsNullOrEmpty(line))
                    {
                        inputs.Add(line);
                        line = file.ReadLine();
                    }
                    if (inputs.Count > 0)
                    {
                        allInputs.Add(inputs);
                    }
                }
                return allInputs;
            }
        }
    }
}
