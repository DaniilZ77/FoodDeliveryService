using System.Collections.ObjectModel;
using static oop2_2023_class4.Program;

namespace oop2_2023_class4;

enum OrderStatus
{
    DEFAULT,
    PROCESSING,
    AWAITINGCOURIER,
    DELIVERING,
    DELIVERED
}

internal interface IEaterService
{
    ReadOnlyCollection<IPlaceReadOnly> GetRestaurantsList();
    Menu GetRestaurantMenuById(long id);
    void SubmitOrder(Order order, Eater eater);
}

internal interface ICourierService
{
    List<IOrderReadOnly> GetAvailableOrders();
    void ChangeOrderStatus(long id, OrderStatus status, Courier courier);
}