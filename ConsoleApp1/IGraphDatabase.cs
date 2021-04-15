//This file contains fragments that You have to fulfill
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using BigTask2.Api;
namespace BigTask2.Data
{
    public interface IGraphDatabase
    {
        //Fill the return type of the method below
        /* */ IIterator GetRoutesFrom(City from);
        City GetByName(string cityName);

        int CountRoutes(City city);
        Route this[City city, int index] { get; }
    }

    class IGraphDatabaseDecorator: IGraphDatabase
    {
        protected IGraphDatabase database;

        public IGraphDatabaseDecorator(IGraphDatabase database)
        {
            this.database = database;
        }

        public virtual IIterator GetRoutesFrom(City from)
        {
            return database.GetRoutesFrom(from);
        }

        public virtual City GetByName(string cityName)
        {
            return database.GetByName(cityName);
        }

        public virtual int CountRoutes(City city)
        {
            return database.CountRoutes(city);
        }

        public virtual Route this[City city, int index]
        {
            get
            {
                return database[city, index];
            }
        }
    }

    class MinPopulationIGraphDatabaseDecorator : IGraphDatabaseDecorator
    {
        protected int minPopulation;

        public MinPopulationIGraphDatabaseDecorator(IGraphDatabase database, int minPopulation) : base(database)
        {
            this.minPopulation = minPopulation;
        }

        public override IIterator GetRoutesFrom(City from)
        {
            return new Iterator(this, from);
        }

        public override int CountRoutes(City city)
        {
            IIterator iterator = base.database.GetRoutesFrom(city);
            int count = 0;
            while (iterator.HasNext())
            {
                Route r = iterator.GetNext();
                if (r.To.Population >= this.minPopulation)
                    count++;
            }
            return count;
        }

        public override Route this[City city, int index]
        {
            get
            {
                if (index >= 0 && index < CountRoutes(city))
                {
                    IIterator iterator = base.database.GetRoutesFrom(city);
                    int count = -1;
                    while (iterator.HasNext())
                    {
                        Route r = iterator.GetNext();
                        if (r.To.Population >= this.minPopulation)
                        {
                            count++;
                            if (count == index)
                                return r;
                        }
                    }
                }
                return null;
            }
        }
    }

    class RestaurantRequiredIGraphDatabaseDecorator: IGraphDatabaseDecorator
    {
        public RestaurantRequiredIGraphDatabaseDecorator(IGraphDatabase database) : base(database) { }

        public override IIterator GetRoutesFrom(City from)
        {
            return new Iterator(this, from);
        }

        public override int CountRoutes(City city)
        {
            IIterator iterator = base.database.GetRoutesFrom(city);
            int count = 0;
            while (iterator.HasNext())
            {
                Route r = iterator.GetNext();
                if (r.To.HasRestaurant) 
                    count++;
            }
            return count;
        }

        public override Route this[City city, int index]
        {
            get
            {
                if (index >= 0 && index < CountRoutes(city))
                {
                    IIterator iterator = base.database.GetRoutesFrom(city);
                    int count = -1;
                    while (iterator.HasNext())
                    {
                        Route r = iterator.GetNext();
                        if (r.To.HasRestaurant) 
                        {
                            count++;
                            if (count == index)
                                return r;
                        }
                    }
                }
                return null;
            }
        }
    }

    class CombinedTwoIGraphDatabaseDecorator: IGraphDatabaseDecorator
    {
        protected IGraphDatabase database2;

        public CombinedTwoIGraphDatabaseDecorator(IGraphDatabase database1, IGraphDatabase database2): base(database1)
        {
            this.database2 = database2;
        }

        public override City GetByName(string cityName)
        {
            City city = base.database.GetByName(cityName);
            if (city == null)
                city = this.database2.GetByName(cityName);
            return city;
        }

        public override IIterator GetRoutesFrom(City from)
        {
            return new Iterator(this, from);
        }

        public override int CountRoutes(City city)
        {
            return base.database.CountRoutes(city) + this.database2.CountRoutes(city);
        }

        public override Route this[City city, int index]
        {
            get
            {
                if (index >= 0 && index < CountRoutes(city))
                {
                    if (index < base.database.CountRoutes(city))
                    {
                        return base.database[city, index];
                    }
                    else
                    {
                        index -= base.database.CountRoutes(city);
                        return this.database2[city, index];
                    }
                }
                return null;
            }
        }
    }

}
