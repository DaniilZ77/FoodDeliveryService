using System.Reflection.Metadata;
using static oop2_2023_class4.Program;
namespace oop2_2023_class4;

internal class Dish
{
    public long RestaurantId { get; private set; }
    public string DishName { get; private set; }
    public Dish(long restaurantId, string dishName)
    {
        RestaurantId = restaurantId;
        DishName = dishName;
    }
}

internal interface IOrderReadOnly
{
    long GetOrderId();
    string GetTargetAddress();
    string? GetTargetPhoneNumber();
}

internal class Order : IOrderReadOnly
{
    private static class OrderIDGenerator
    {
        private static long lastId = 0;
        public static long GetID() => ++lastId;
    }
    private readonly long _orderId;
    private readonly string _targetAddress;
    public OrderStatus OrderStatus { get; private set; }
    private Eater? _eater = null;
    private Courier? _courier = null;
    public List<Dish> Dishes { get; }
    public Order(List<Dish> dishes, string targetAddress, Eater eater)
    {
        Dishes = dishes;
        _orderId = OrderIDGenerator.GetID();
        OrderStatus = OrderStatus.DEFAULT;
        _eater = eater;
        _targetAddress = targetAddress;
    }
    public void SetCourier(Courier courier) => _courier = courier;
    public void ChangeOrderStatus(long courierId, OrderStatus status)
    {
        if (_courier == null)
        {
            throw new ArgumentException("Этот заказ еще не взял ни один курьер");
        }
        if (courierId != _courier.CourierId)
        {
            throw new ArgumentException("Идентификатор курьера, который взял этот заказ и того курьера, который запрашивает изменения не совпадают");
        }
        OrderStatus = status;
    }
    public void SetOrderReadyForCourier()
    {
        OrderStatus = OrderStatus.AWAITINGCOURIER;
    }
    public void SetOrderIsProcessing()
    {
        OrderStatus = OrderStatus.PROCESSING;
    }
    public long GetOrderId()
    {
        return _orderId;
    }

    public string GetTargetAddress()
    {
        return _targetAddress;
    }

    public string GetTargetPhoneNumber()
    {
        if (_eater == null)
        {
            throw new ArgumentException("Переменная _eater еще не инициализирована");
        }
        return _eater.PhoneNumber;
    }
}

internal class Menu
{
    public long RestaurantId { get; private set; }
    private readonly List<Dish> _dishes;
    public Dish GetDish(string dishName)
    {
        Dish? dish = null;
        if ((dish = _dishes.FirstOrDefault(dish => dish.DishName == dishName)) == null)
        {
            throw new ArgumentException($"К сожалению данного блюда: {dishName} не сущесвтует");
        }
        return dish;
    }

    public Menu(long restaurantId, List<string> dishes)
    {
        RestaurantId = restaurantId;
        _dishes = dishes.Select(dish => new Dish(restaurantId, dish)).ToList();
    }

    public Menu(long restaurantId, List<Dish> dishes)
    {
        RestaurantId = restaurantId;
        _dishes = dishes;
    }
}