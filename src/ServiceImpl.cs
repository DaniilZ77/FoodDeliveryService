
using System.Collections.ObjectModel;
using static oop2_2023_class4.Program;

namespace oop2_2023_class4;

internal class Service : IEaterService, ICourierService
{
    private IStorage<Place>? _placeStorage = null;
    private IStorage<Eater>? _eaterStorage = null;
    private IStorage<Courier>? _courierStorage = null;

    public void SetPlaceStorage(IStorage<Place> storage) => _placeStorage = storage;

    public void SetEaterStorage(IStorage<Eater> storage) => _eaterStorage = storage;

    public void SetCourierStorage(IStorage<Courier> storage) => _courierStorage = storage;


    public void SubmitOrder(Order order, Eater eater)
    {
        if (_eaterStorage == null)
        {
            throw new ArgumentException("CourierStorage еще не инициализирован");
        }
        else if (!_eaterStorage.GetItems().Any(item => item == eater))
        {
            throw new ArgumentException($"Пользователь с идентификатором {eater.EaterId} еще не заререстрирован");
        }
        Dictionary<long, List<Dish>> orders = [];
        foreach (var dishesItem in order.Dishes)
        {
            if (!orders.TryGetValue(dishesItem.RestaurantId, out List<Dish>? value))
            {
                value = [];
                orders[dishesItem.RestaurantId] = value;
            }
            value.Add(dishesItem);
        }

        foreach (var (id, items) in orders)
        {
            try
            {
                _placeStorage!.GetById(id).AddOrder(new Order(items, order.GetTargetAddress(), eater));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public void ChangeOrderStatus(long id, OrderStatus status, Courier courier)
    {
        if (_courierStorage == null)
        {
            throw new ArgumentException("CourierStorage еще не инициализирован");
        }
        else if (!_courierStorage.GetItems().Any(item => item == courier))
        {
            throw new ArgumentException($"Курьера с идентификатором {courier.CourierId} не существует");
        }
        try
        {
            _placeStorage!.GetItems().First(place => place.GetOrdersList().Any(order => order.GetOrderId() == id)).ChangeOrderStatus(id, status, courier);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<IOrderReadOnly> GetAvailableOrders()
    {
        if (_placeStorage == null)
        {
            throw new ArgumentException("PlaceStorage еще не инициализирован");
        }
        return _placeStorage.GetItems().SelectMany(place => place.GetOrdersList().Where(order => order.OrderStatus == OrderStatus.AWAITINGCOURIER).Select(order => (IOrderReadOnly)order)).ToList();
    }

    public Menu GetRestaurantMenuById(long id)
    {
        return _placeStorage!.GetById(id).Menu;
    }

    public ReadOnlyCollection<IPlaceReadOnly> GetRestaurantsList()
    {
        if (_placeStorage == null)
        {
            throw new ArgumentException("PlaceStorage еще не инициализирован");
        }
        return _placeStorage.GetItems().Select(item => (IPlaceReadOnly)item).ToList().AsReadOnly();
    }

    public void RegisterCourier(string name, string passport)
    {
        if (_courierStorage == null)
        {
            throw new Exception("CourierStorage еще не инициализирован");
        }
        _courierStorage.Save(new Courier(name, passport, this));
    }

    public void RegisterCourier(Courier courier)
    {
        if (_courierStorage == null)
        {
            throw new Exception("CourierStorage еще не инициализирован");
        }
        _courierStorage.Save(courier);
    }

    public void RegisterEater(string name, string phoneNumber)
    {
        if (_eaterStorage == null)
        {
            throw new ArgumentException($"EaterStorage еще не инициализирован");
        }
        _eaterStorage.Save(new Eater(name, phoneNumber, this));
    }

    public void RegisterEater(Eater eater)
    {
        if (_eaterStorage == null)
        {
            throw new ArgumentException($"EaterStorage еще не инициализирован");
        }
        _eaterStorage.Save(eater);
    }

    public void RegisterPlace(Place place)
    {
        if (_placeStorage == null)
        {
            throw new ArgumentException($"PlaceStorage еще не инициализирован");
        }
        _placeStorage.Save(place);
    }

    public void RegisterPlace(string name, string address, Menu menu)
    {
        if (_placeStorage == null)
        {
            throw new ArgumentException($"PlaceStorage еще не инициализирован");
        }
        _placeStorage.Save(new Place(name, address, menu));
    }
}