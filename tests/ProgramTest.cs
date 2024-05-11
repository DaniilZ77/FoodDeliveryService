using System.Reflection;
using NUnit.Framework;
using static oop2_2023_class4.Program;

namespace oop2_2023_class4;
class ProgramTest
{
    [SetUp]
    public void Setup()
    {
        MockData.Service.SetPlaceStorage(MockData.PlaceStorage);
        MockData.Service.SetCourierStorage(MockData.CourierStorage);
        MockData.Service.SetEaterStorage(MockData.EaterStorage);
    }

    [Test]
    public void PlaceStorageSuccessAccessTest()
    {
        var placeStorage = new PlaceStorage();
        List<Place> places = [];
        for (var i = 0; i < MockData.RestaurantNames.Count; ++i)
        {
            var place = new Place(MockData.RestaurantNames[i], MockData.Addresses[i], new Menu(1, new List<string>()));
            placeStorage.Save(place);
            places.Add(place);
        }

        foreach (var place in places)
        {
            Assert.That(placeStorage.GetById(place.PlaceId), Is.EqualTo(place));
        }
    }

    [Test]
    public void PlaceStorageFailAccessTest()
    {
        var placeStorage = new PlaceStorage();
        Assert.Multiple(() =>
        {
            Assert.Catch<ArgumentException>(() => placeStorage.GetById(77794660));
            Assert.Catch<ArgumentException>(() => placeStorage.GetById(17453678));
            Assert.Catch<ArgumentException>(() => placeStorage.GetById(43084893));
        });
    }

    [Test]
    public void CourierStorageSuccessAccessTest()
    {
        var courierStorage = new Program.CourierStorage();
        List<Courier> couriers = [];
        for (var i = 0; i < MockData.Names.Count; ++i)
        {
            var courier = new Courier(MockData.Names[i], MockData.Passports[i], new Service());
            courierStorage.Save(courier);
            couriers.Add(courier);
        }

        foreach (var courier in couriers)
        {
            Assert.That(courierStorage.GetById(courier.CourierId), Is.EqualTo(courier));
        }
    }

    [Test]
    public void CourierStorageFailAccessTest()
    {
        var courierStorage = new CourierStorage();
        Assert.Multiple(() =>
        {
            Assert.Catch<ArgumentException>(() => courierStorage.GetById(6333957));
            Assert.Catch<ArgumentException>(() => courierStorage.GetById(61925889));
            Assert.Catch<ArgumentException>(() => courierStorage.GetById(54188400));
        });
    }

    [Test]
    public void EaterStorageSuccessAccessTest()
    {
        var eaterStorage = new EaterStorage();
        List<Eater> eaters = [];
        for (var i = 0; i < MockData.Names.Count; ++i)
        {
            var eater = new Eater(MockData.Names[i], MockData.PhoneNumbers[i], new Service());
            eaterStorage.Save(eater);
            eaters.Add(eater);
        }

        foreach (var eater in eaters)
        {
            Assert.That(eaterStorage.GetById(eater.EaterId), Is.EqualTo(eater));
        }
    }

    [Test]
    public void EaterStorageFailAccessTest()
    {
        var eaterStorage = new EaterStorage();
        Assert.Multiple(() =>
        {
            Assert.Catch<ArgumentException>(() => eaterStorage.GetById(85978946));
            Assert.Catch<ArgumentException>(() => eaterStorage.GetById(47099419));
            Assert.Catch<ArgumentException>(() => eaterStorage.GetById(31859868));
        });
    }

    [Test]
    public void Constructor_SetsUniquePlaceIds()
    {
        var place1 = new Place("Place One", "Address One", MockData.Menus[0]);
        var place2 = new Place("Place Two", "Address Two", MockData.Menus[0]);

        Assert.AreNotEqual(place1.PlaceId, place2.PlaceId);
    }

    [Test]
    public void CopyConstructor_CopiesAllProperties()
    {
        var place1 = new Place("Original", "Original Address", MockData.Menus[1]);
        var place2 = new Place(place1);


        Assert.AreNotEqual(place1, place2);
        Assert.AreEqual(place1.PlaceId, place2.PlaceId);
        Assert.AreEqual(place1.GetName(), place2.GetName());
        Assert.AreEqual(place1.GetAddress(), place2.GetAddress());
        Assert.AreEqual(place1.Menu, place2.Menu);
    }

    [Test]
    public void RemoveOrder_DecreasesOrdersCount()
    {
        var place = new Place(MockData.Places[1]);
        var order = MockData.Orders[1];
        place.AddOrder(MockData.Orders[1]);
        long orderId = order.GetOrderId();
        place.RemoveOrder(orderId);
        Assert.That(place.GetOrdersList().Count, Is.EqualTo(0));
    }

    [Test]
    public void RemoveOrder_DoesNothingWhenDoesNotHaveOrder()
    {
        var place = new Place(MockData.Places[1]);
        place.AddOrder(MockData.Orders[1]);
        place.RemoveOrder(-1);
        Assert.That(place.GetOrdersList().Count, Is.EqualTo(1));
    }

    [Test]
    public void AddOrder_IncreasesOrderCount()
    {
        var place = new Place(MockData.Places[0]);
        place.AddOrder(MockData.Orders[0]);
        place.AddOrder(MockData.Orders[1]);
        Assert.That(place.GetOrdersList().Count, Is.EqualTo(2));
    }

    [Test]
    public void GetOrdersList_ReturnsAllAddedOrders()
    {
        var place = new Place(MockData.Places[0]);
        place.AddOrder(MockData.Orders[0]);
        place.AddOrder(MockData.Orders[1]);

        var orders = place.GetOrdersList();

        Assert.That(orders, Has.Count.EqualTo(2));
        Assert.That(orders, Contains.Item(MockData.Orders[0]));
        Assert.That(orders, Contains.Item(MockData.Orders[1]));
    }

    [Test]
    public void GetDishesOfOrder_ReturnsCorrectDishesForOrder()
    {
        var place = new Place(MockData.Places[2]);
        var order = MockData.Orders[0];
        place.AddOrder(order);
        place.AddOrder(MockData.Orders[1]);
        place.AddOrder(MockData.Orders[2]);
        var resultDishes = place.GetDishesOfOrder(order.GetOrderId());
        var expectedDishes = new List<Dish> { MockData.Dishes[0], MockData.Dishes[1] };
        Assert.That(resultDishes, Is.EquivalentTo(expectedDishes));
    }

    [Test]
    public void GetDishesOfOrder_ThrowsArgumentException_WhenOrderDoesNotExist()
    {
        var place = MockData.Places[0];
        long nonExistingOrderId = 999;
        var ex = Assert.Throws<ArgumentException>(() => { place.GetDishesOfOrder(nonExistingOrderId); });
        Assert.That(ex!.Message, Is.EqualTo($"К сожалению заказа с таким id: {nonExistingOrderId} не существует"));
    }

    [Test]
    public void ChangeOrderStatus_ToProcessing_SetsStatusCorrectly()
    {
        var place = new Place(MockData.Places[0]);
        var order = MockData.Orders[0];
        place.AddOrder(order);
        place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.PROCESSING);
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.PROCESSING));
    }

    [Test]
    public void ChangeOrderStatus_ToAwaitingCourier_SetsStatusCorrectly()
    {
        var place = new Place(MockData.Places[0]);
        var order = MockData.Orders[0];
        place.AddOrder(order);
        place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.AWAITINGCOURIER);
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.AWAITINGCOURIER));
    }

    [Test]
    public void ChangeOrderStatus_ThrowsException_ForInvalidStatusChange()
    {
        var place = new Place(MockData.Places[0]);
        var order = MockData.Orders[0];
        place.AddOrder(order);
        Assert.Throws<ArgumentException>(() => place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.DELIVERED), "Ресторан не имеет право изменять статус заказа на DELIVERED");
    }

    [Test]
    public void ChangeOrderStatus_ThrowsException_WhenOrderDoesNotExist()
    {
        var place = new Place(MockData.Places[0]);
        var order = MockData.Orders[0];
        long nonExistingOrderId = 999;
        Assert.Throws<ArgumentException>(() => place.ChangeOrderStatus(nonExistingOrderId, OrderStatus.PROCESSING), "order is not set.");
    }

    [Test]
    public void ChangeOrderStatus_UpdatesStatus_WhenConditionsAreMet()
    {
        var place = new Place(MockData.Places[3]);
        var courier = MockData.Couriers[1];
        var order = MockData.Orders[2];
        place.AddOrder(order);
        place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.AWAITINGCOURIER);

        place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.DELIVERING, courier);

        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.DELIVERING));
    }

    [Test]
    public void ChangeOrderStatus_Courier_ThrowsException_WhenOrderDoesNotExist()
    {
        var place = new Place(MockData.Places[0]);
        var courier = MockData.Couriers[3];
        long nonExistingOrderId = 999;
        Assert.Throws<ArgumentException>(() => place.ChangeOrderStatus(nonExistingOrderId, OrderStatus.DELIVERING, courier), "order еще не инициализирован");
    }

    [Test]
    public void ChangeOrderStatus_ThrowsException_ForUnauthorizedStatusChange()
    {
        var place = new Place(MockData.Places[0]);
        var courier = MockData.Couriers[2];
        var order = MockData.Orders[1];
        place.AddOrder(order);

        Assert.Throws<ArgumentException>(() => place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.PROCESSING, courier),
            "Курьер не имеет право изменять статус заказа на PROCESSING");
    }

    [Test]
    public void GetAvailableOrders_ReturnsCorrectOrders()
    {
        var place = new Place(MockData.Places[2]);
        MockData.Service.RegisterPlace(place);
        var order1 = MockData.Orders[2];
        place.AddOrder(order1);
        place.ChangeOrderStatus(order1.GetOrderId(), OrderStatus.AWAITINGCOURIER);
        var order2 = MockData.Orders[1];
        place.AddOrder(order2);
        place.ChangeOrderStatus(order2.GetOrderId(), OrderStatus.AWAITINGCOURIER);

        var courier = MockData.Couriers[2];
        MockData.Service.RegisterCourier(courier);
        var availableOrders = courier.GetAvailableOrders();
        Assert.That(availableOrders, Is.Not.Empty);
    }

    [Test]
    public void ChangeOrderStatus_ThrowsException_WhenNoOrderIsAssigned()
    {
        var courier = MockData.Couriers[1];

        Assert.Throws<ArgumentException>(() => courier.ChangeOrderStatus(OrderStatus.DELIVERED), "К сожалению заказ еще не сформирован");
    }

    [Test]
    public void CourierChangeOrderStatus_UpdatesStatus_WhenConditionsAreMet()
    {
        var place = new Place(MockData.Places[3]);
        MockData.Service.RegisterPlace(place);
        var courier = MockData.Couriers[3];
        MockData.Service.RegisterCourier(courier);
        var order = MockData.Orders[2];
        place.AddOrder(order);
        place.ChangeOrderStatus(order.GetOrderId(), OrderStatus.AWAITINGCOURIER);
        courier.ChooseOrder(order);
        courier.ChangeOrderStatus(OrderStatus.DELIVERING);
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.DELIVERING));
        courier.ChangeOrderStatus(OrderStatus.DELIVERED);
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.DELIVERED));
    }

    [Test]
    public void AddDish_AddsDishCorrectly()
    {
        var eater = MockData.Eaters[2];
        var dish = MockData.Dishes[1];
        eater.AddDish(dish);
        Assert.That(eater.GetDishes(), Contains.Item(dish));
        Assert.That(eater.GetDishes().Count, Is.EqualTo(1));
    }

    [Test]
    public void SubmitOrder_CallsEaterServiceWithCorrectOrder()
    {
        var eater = MockData.Eaters[2];
        var dish = MockData.Dishes[1];
        var place = MockData.Places[0];
        eater.AddDish(dish);
        MockData.Service.RegisterPlace(place);
        MockData.Service.RegisterEater(eater);
        eater.SubmitOrder(MockData.Addresses[2]);
        var orderId = place.GetOrdersList().First().GetOrderId();
        Assert.That(place.GetDishesOfOrder(orderId), Contains.Item(dish));
    }

    [Test]
    public void GetRestaurantsList_ReturnsCorrectData()
    {
        var eater = MockData.Eaters[0];
        var place = MockData.Places[2];
        MockData.Service.RegisterPlace(place);
        MockData.Service.RegisterEater(eater);
        Assert.That(eater.GetRestaurantsList(), Is.Not.Empty);
    }

    [Test]
    public void GetRestaurantMenuById_ReturnsCorrectMenu()
    {
        var eater = MockData.Eaters[1];
        var place = MockData.Places[1];
        MockData.Service.RegisterPlace(place);
        MockData.Service.RegisterEater(eater);
        Assert.That(eater.GetRestaurantMenuById(place.PlaceId), Is.EqualTo(MockData.Menus[1]));
    }

}