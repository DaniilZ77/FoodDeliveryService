using System.Collections;
using System.Collections.ObjectModel;
using System.Dynamic;

namespace oop2_2023_class4
{
    internal class Program
    {
        public interface IStorage<T>
        {
            T GetById(long id);
            void Save(T value);
            ReadOnlyCollection<T> GetItems();
        }

        public class PlaceStorage : IStorage<Place>
        {
            private readonly Dictionary<long, Place> _places = [];
            public Place GetById(long id)
            {
                if (!_places.TryGetValue(id, out Place? place))
                {
                    throw new ArgumentException($"К сожалению такого ресторана c таким id: {id} не существует");
                }
                return place;
            }
            public ReadOnlyCollection<Place> GetItems()
            {
                return _places.Values.ToList().AsReadOnly();
            }

            public void Save(Place value) => _places.TryAdd(value.PlaceId, value);
        }

        public class CourierStorage : IStorage<Courier>
        {
            private readonly List<Courier> _couriers = [];
            public Courier GetById(long id)
            {
                Courier? courier = null;
                if ((courier = _couriers.FirstOrDefault(courier => courier.CourierId == id)) == null)
                {
                    throw new ArgumentException($"К сожалению курьера с таким id: {id} не существует");
                }
                return courier;
            }

            public ReadOnlyCollection<Courier> GetItems()
            {
                return _couriers.AsReadOnly();
            }

            public void Save(Courier value) => _couriers.Add(value);
        }

        public class EaterStorage : IStorage<Eater>
        {
            private readonly List<Eater> _eaters = [];
            public Eater GetById(long id)
            {
                Eater? eater = null;
                if ((eater = _eaters.FirstOrDefault(courier => courier.EaterId == id)) == null)
                {
                    throw new ArgumentException($"К сожалению курьера с таким id: {id} не существует");
                }
                return eater;
            }
            public ReadOnlyCollection<Eater> GetItems()
            {
                return _eaters.AsReadOnly();
            }

            public void Save(Eater value) => _eaters.Add(value);
        }

        internal interface IPlaceReadOnly
        {
            string GetAddress();
            string GetName();
        }

        public class Place : IPlaceReadOnly
        {
            private static class PlaceIDGenerator
            {
                private static long lastId = 0;
                public static long GetID() => ++lastId;
            }
            private readonly List<Order> _orders = [];
            public Menu Menu { get; }
            public long PlaceId { get; private set; }
            private readonly string _name;

            private readonly string _address;

            public override string ToString()
            {
                return string.Join(", ", PlaceId.ToString(), _name, _address);
            }

            public void AddOrder(Order order)
            {
                _orders.Add(order);
            }

            public void RemoveOrder(long id)
            {
                IEnumerable<Order>? collection = null;
                if (!(collection = _orders.Where(order => order.GetOrderId() == id)).Any())
                {
                    Console.WriteLine($"К сожалению заказа с таким id: {id} не существует");
                    return;
                }
                _orders.Remove(collection.First());
            }

            public List<Order> GetOrdersList()
            {
                return _orders;
            }

            public List<Dish> GetDishesOfOrder(long orderId)
            {
                if (!_orders.Any(order => orderId == order.GetOrderId()))
                {
                    throw new ArgumentException($"К сожалению заказа с таким id: {orderId} не существует");
                }
                return _orders.Find(order => orderId == order.GetOrderId())!.Dishes;
            }

            public void ChangeOrderStatus(long orderId, OrderStatus status)
            {
                var order = _orders.Find(order => order.GetOrderId() == orderId);
                if (order == null)
                {
                    throw new ArgumentException($"{nameof(order)} еще не инициализирован");
                }
                switch (status)
                {
                    case OrderStatus.PROCESSING:
                        order.SetOrderIsProcessing();
                        break;
                    case OrderStatus.AWAITINGCOURIER:
                        order.SetOrderReadyForCourier();
                        break;
                    default:
                        throw new ArgumentException($"Ресторан не имеет право изменять статус заказа на {status}");
                }
            }

            public void ChangeOrderStatus(long orderId, OrderStatus status, Courier courier)
            {
                var order = _orders.Find(order => order.GetOrderId() == orderId);
                if (order == null)
                {
                    throw new ArgumentException($"{nameof(order)} is not set.");
                }
                if (status != OrderStatus.DELIVERING && status != OrderStatus.DELIVERED)
                {
                    throw new ArgumentException($"Курьер не имеет право изменять статус заказа на {status}");
                }
                if (order.OrderStatus == OrderStatus.AWAITINGCOURIER)
                {
                    order.SetCourier(courier);
                }
                order.ChangeOrderStatus(courier.CourierId, status);
            }

            public string GetAddress()
            {
                return _address;
            }

            public string GetName()
            {
                return _name;
            }

            public Place(string name, string address, Menu menu)
            {
                PlaceId = PlaceIDGenerator.GetID();
                _name = name;
                _address = address;
                Menu = menu;
            }

            public Place(Place place)
            {
                PlaceId = place.PlaceId;
                _name = place.GetName();
                _address = place.GetAddress();
                Menu = place.Menu;
            }
        }

        public class Courier
        {
            private readonly ICourierService _courierService;
            private IOrderReadOnly? _currentOrder = null;
            private static class CourierIDGenerator
            {
                private static long lastId = 0;
                public static long GetID() => ++lastId;
            }
            public long CourierId { get; private set; }
            public string FirstName { get; private set; }
            public string LastName { get; private set; }

            private readonly string _passport;
            public Courier(string name, string passport, ICourierService courierService)
            {
                CourierId = CourierIDGenerator.GetID();
                var nameSplited = name.Split(' ');
                FirstName = nameSplited.First();
                LastName = nameSplited.Last();
                _passport = passport;
                _courierService = courierService;
            }

            public List<IOrderReadOnly> GetAvailableOrders()
            {
                return _courierService.GetAvailableOrders();
            }

            public void ChangeOrderStatus(OrderStatus status)
            {
                if (_currentOrder == null)
                {
                    throw new ArgumentException("К сожалению заказ еще не сформирован");
                }
                _courierService.ChangeOrderStatus(_currentOrder.GetOrderId(), status, this);
            }

            public void ChooseOrder(IOrderReadOnly order)
            {
                _currentOrder = order;
            }

            public override string ToString()
            {
                return string.Join(", ", CourierId, _passport, FirstName, LastName);
            }
        }

        public class Eater
        {
            private readonly IEaterService _eaterService;
            private readonly List<Dish> _order = [];
            private static class EaterIDGenerator
            {
                private static long lastId = 0;
                public static long GetID() => ++lastId;
            }
            public long EaterId { get; private set; }
            public string FirstName { get; private set; }
            public string LastName { get; private set; }

            public string PhoneNumber { get; private set; }

            public Eater(string name, string phoneNumber, IEaterService eaterService)
            {
                EaterId = EaterIDGenerator.GetID();
                var nameSplited = name.Split(' ');
                FirstName = nameSplited.First();
                LastName = nameSplited.Last();
                _eaterService = eaterService;
                PhoneNumber = phoneNumber;
            }

            public void AddDish(Dish dish)
            {
                _order.Add(dish);
            }

            public ReadOnlyCollection<Dish> GetDishes()
            {
                return _order.AsReadOnly();
            }

            public void SubmitOrder(string targetAddress)
            {
                var order = new Order(_order, targetAddress, this);
                _eaterService.SubmitOrder(order, this);
            }

            public ReadOnlyCollection<IPlaceReadOnly> GetRestaurantsList()
            {
                return _eaterService.GetRestaurantsList();
            }

            public Menu GetRestaurantMenuById(long id)
            {
                return _eaterService.GetRestaurantMenuById(id);
            }

            public override string ToString()
            {
                return string.Join(", ", EaterId, PhoneNumber, FirstName, LastName);
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
        }
    }
}