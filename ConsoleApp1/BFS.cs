﻿//This file contains fragments that You have to fulfill
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Api;
using BigTask2.Data;
using System.Collections.Generic;
using BigTask2.Interfaces;
using BigTask2.Problems;

namespace BigTask2.Algorithms
{
	public class BFS
	{
		public IEnumerable<Route> Solve(IGraphDatabase graph, City from, City to)
		{
			Dictionary<City, Route> routes = new Dictionary<City, Route>();
			routes[from] = null;
			Queue<City> queue = new Queue<City>();
			queue.Enqueue(from);
			do
			{
				City city = queue.Dequeue();
                /*
				 * For each outgoing route from city...
				 */
                IIterator iterator = graph.GetRoutesFrom(city);
                while (iterator.HasNext()) 
				{
                    Route route = iterator.GetNext(); /* Change to current Route*/
					if (routes.ContainsKey(route.To))
					{
						continue;
					}
					routes[route.To] = route;
					if (route.To == to)
					{
						break;
					}
					queue.Enqueue(route.To);
				}
			} while (queue.Count > 0);
			if (!routes.ContainsKey(to))
			{
				return null;
			}
			List<Route> result = new List<Route>();
			for (Route route = routes[to]; route != null; route = routes[route.From])
			{
				result.Add(route);
			}
			result.Reverse();
			return result;
		}
    }
}
