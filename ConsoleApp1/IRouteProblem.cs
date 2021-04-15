//This file Can be modified
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using System.Collections.Generic;
using BigTask2.Data;
using BigTask2.Api;
using BigTask2.Problems;
using BigTask2.Algorithms;

namespace BigTask2.Interfaces
{
    public interface IRouteProblem
	{
        string Type { get; }
        IGraphDatabase Graph { get; set; }
        bool Solved { get; set; }
        string Algorithm { get; set; }
        IEnumerable<Route> Accept(ISolver visitor);
        IRouteProblem Visit(Request request);
    }

    public interface ISolver
    {
        IEnumerable<Route> Visit(CostProblem costProblem);
        IEnumerable<Route> Visit(TimeProblem timeProblem);
    }

    class BFSSolver: ISolver
    {
        private BFS algorithm = new BFS();
        private string description = "BFS";

        public IEnumerable<Route> Visit(CostProblem costProblem)
        {
            if (costProblem.Algorithm == this.description)
                costProblem.Solved = true;
            return this.algorithm.Solve(costProblem.Graph, costProblem.Graph.GetByName(costProblem.From), costProblem.Graph.GetByName(costProblem.To));
        }

        public IEnumerable<Route> Visit(TimeProblem timeProblem)
        {
            if (timeProblem.Algorithm == this.description)
                timeProblem.Solved = true;
            return this.algorithm.Solve(timeProblem.Graph, timeProblem.Graph.GetByName(timeProblem.From), timeProblem.Graph.GetByName(timeProblem.To));
        }
    }

    class DFSSolver: ISolver
    {
        private DFS algorithm = new DFS();
        private string description = "DFS";

        public IEnumerable<Route> Visit(CostProblem costProblem)
        {
            if (costProblem.Algorithm == this.description)
                costProblem.Solved = true;
            return this.algorithm.Solve(costProblem.Graph, costProblem.Graph.GetByName(costProblem.From), costProblem.Graph.GetByName(costProblem.To));
        }

        public IEnumerable<Route> Visit(TimeProblem timeProblem)
        {
            if (timeProblem.Algorithm == this.description)
                timeProblem.Solved = true;
            return this.algorithm.Solve(timeProblem.Graph, timeProblem.Graph.GetByName(timeProblem.From), timeProblem.Graph.GetByName(timeProblem.To));
        }
    }

    class DijkstraSolver: ISolver
    {
        private DijkstraCost costAlgorithm = new DijkstraCost();
        private DijkstraTime timeAlgorithm = new DijkstraTime();
        private string description = "Dijkstra";

        public IEnumerable<Route> Visit(CostProblem costProblem)
        {
            if (costProblem.Algorithm == this.description)
                costProblem.Solved = true;
            return this.costAlgorithm.Solve(costProblem.Graph, costProblem.Graph.GetByName(costProblem.From), costProblem.Graph.GetByName(costProblem.To));
        }

        public IEnumerable<Route> Visit(TimeProblem timeProblem)
        {
            if (timeProblem.Algorithm == this.description)
                timeProblem.Solved = true;
            return this.timeAlgorithm.Solve(timeProblem.Graph, timeProblem.Graph.GetByName(timeProblem.From), timeProblem.Graph.GetByName(timeProblem.To));
        }
    }
}
