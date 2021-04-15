//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

using System;
using System.Collections.Generic;
using System.Text;
using BigTask2.Api;

namespace BigTask2.Data
{
    public interface IIterator
    {
        void Reset();
        Route GetNext();
        bool HasNext();
    }

    class Iterator: IIterator
    {
        private IGraphDatabase database;
        private City city;
        private int index = 0;

        public Iterator(IGraphDatabase database, City city)
        {
            this.database = database;
            this.city = city;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public Route GetNext()
        {
            if (HasNext())
            {
                Route tmp = database[city, index];
                index++;
                return tmp;
            }
            return null;
        }

        public bool HasNext()
        {
            return index < database.CountRoutes(this.city);
        }
    }
}
