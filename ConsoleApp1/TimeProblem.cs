//This file Can be modified
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Data;
using BigTask2.Interfaces;
using System.Collections.Generic;
using BigTask2.Api;

namespace BigTask2.Problems
{
	public class TimeProblem : IRouteProblem
	{
        private string type = "Time";

        public string From, To;

		public TimeProblem(string from, string to)
		{
			From = from;
			To = to;
		}

        public TimeProblem() { }

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
                return new TimeProblem(request.From, request.To);
            return null;
        }
    }
}
