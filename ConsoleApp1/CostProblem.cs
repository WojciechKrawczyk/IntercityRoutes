//This file Can be modified
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Data;
using BigTask2.Interfaces;
using System.Collections.Generic;
using BigTask2.Api;

namespace BigTask2.Problems
{
	public class CostProblem : IRouteProblem
	{
        private string type = "Cost";

		public string From, To;

		public CostProblem(string from, string to)
		{
			From = from;
			To = to;
		}

        public CostProblem() { }

        public string Type { get { return type; } }

        public IGraphDatabase Graph { get; set; }

        public bool Solved { get; set; }

        public string Algorithm { get; set; }

        public IEnumerable<Route> Accept(ISolver visitor)
        {
            IEnumerable<Route> routes = visitor.Visit(this);
            return routes;
        }

        public IRouteProblem Visit(Request request)
        {
            if (request.Problem == this.type)
            {
                return new CostProblem(request.From, request.To);
            }
            return null;
        }
    }
}
