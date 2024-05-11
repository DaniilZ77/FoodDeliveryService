using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oop2_2023_class4
{
    internal class Program
    {
        public class Place
        {
            private static class PlaceIDGenerator
            {
                private static Int64 lastId = 0;
                public static Int64 GetID() => ++lastId;
            }
            public Int64 place_id { get; private set; }
            public string name { get; set; }

            public override string ToString()
            {
                return string.Join(", ", place_id.ToString(), name);
            }

            public Place(string name)
            {
                place_id = PlaceIDGenerator.GetID();
                this.name = name;
            }

            public Place(Place place)
            {
                place_id = place.place_id;
                name = place.name;
            }
        }

        public class Courier
        {
            private static class CourierIDGenerator
            {
                private static Int64 lastId = 0;
                public static Int64 GetID() => ++lastId;
            }
            public Int64 courier_id { get; private set; }
            public string firstName {
                get;
                private set;
            }
            public string lastName {
                get;
                private set;
            }

            public string passport
            {
                get;
                private set;
            }
            
            public Courier(string name, string passport)
            {
                courier_id = CourierIDGenerator.GetID();
                var name_spleted = name.Split(' ');
                firstName = name_spleted.First();
                lastName = name_spleted.Last();
                this.passport = passport;
            }

            public override string ToString()
            {
                return string.Join(", ",courier_id,passport,firstName,lastName);
            }
        }
        
        public class Eater
        {
            private static class EaterIDGenerator
            {
                private static Int64 lastId = 0;
                public static Int64 GetID() => ++lastId;
            }
            public Int64 eater_id { get; private set; }
            public string firstName {
                get;
                private set;
            }
            public string lastName {
                get;
                private set;
            }

            public string phone_number
            {
                get;
                private set;
            }
            
            public Eater(string name, string phone_number)
            {
                eater_id = EaterIDGenerator.GetID();
                var name_spleted = name.Split(' ');
                firstName = name_spleted.First();
                lastName = name_spleted.Last();
                this.phone_number = phone_number;
            }

            public override string ToString()
            {
                return string.Join(", ",eater_id,phone_number,firstName,lastName);
            }
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
        }
    }
}