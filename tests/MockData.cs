using static oop2_2023_class4.Program;

namespace oop2_2023_class4;
public static class MockData
{
    internal static readonly List<string> RestaurantNames =
    [
        "Звездный Оазис",
        "Блюдо Мира",
        "Вкусы Солнца",
        "Лагуна Вкусов",
        "Сапфировая Ложка",
        "Мистический Вкус",
        "Гурманский Рай",
        "Лесные Деликатесы",
        "Уголок Шефа",
        "Платиновый Берег"
    ];

    internal static readonly List<string> Addresses =
    [
        "Россия, г. Москва, Новый пер., д. 8 кв.189",
        "Россия, г. Якутск, Чапаева ул., д. 14 кв.74",
        "Россия, г. Якутск, Чапаева ул., д. 14 кв.74",
        "Россия, г. Елец, Дружная ул., д. 4 кв.93",
        "Россия, г. Тольятти, Рабочая ул., д. 22 кв.198",
        "Россия, г. Батайск, Радужная ул., д. 22 кв.130",
        "Россия, г. Кызыл, Ленинская ул., д. 22 кв.115",
        "Россия, г. Первоуральск, Зеленая ул., д. 22 кв.123",
        "Россия, г. Копейск, Колхозная ул., д. 23 кв.110",
        "Россия, г. Копейск, Колхозная ул., д. 23 кв.110"
    ];

    internal static readonly List<string> Names =
    [
        "Мечтилд Дюпре",
        "Хелфрид Кромбергер",
        "Лис Морель",
        "Адалард Комиссаров",
        "Эрхард Адэртад",
        "Максимиллиан Сэффман",
        "Каори Клиффорд",
        "Этей Орр",
        "Клавдий Михайлов",
        "Эвда Лиувилль"
    ];

    internal static readonly List<string> Passports =
    [
        "67 26 327321, выдан  ГУ МВД России по г. Нижнему Новгороду, к/п 473-277",
        "56 96 924400, выдан  ГУ МВД России по г. Екатеринбургу, к/п 773-245",
        "29 15 202145, выдан  ГУ МВД России по г. Ижевску, к/п 041-896",
        "85 20 459631, выдан  ГУ МВД России по г. Воронежу, к/п 358-508",
        "14 25 999619, выдан  ГУ МВД России по г. Казани, к/п 930-932",
        "64 34 780740, выдан  ГУ МВД России по г. Воронежу, к/п 175-289",
        "48 39 798420, выдан  ГУ МВД России по г. Саратову, к/п 906-815",
        "57 24 334139, выдан  ГУ МВД России по г. Тюмени, к/п 224-909",
        "02 05 575728, выдан  ГУ МВД России по г. Ижевску, к/п 240-884",
        "62 77 773676, выдан  ГУ МВД России по г. Волгограду, к/п 813-829"
    ];

    internal static readonly List<string> PhoneNumbers =
    [
        "799(24)671-94-84",
        "9(6653)950-70-93",
        "6(15)402-07-30",
        "2(413)794-49-94",
        "03(2993)085-38-37",
        "493(0798)426-37-24",
        "2(0215)916-84-37",
        "208(88)523-16-04",
        "1(7298)289-86-42",
        "34(272)982-29-83"
    ];

    internal static readonly List<Dish> Dishes =
    [
        new Dish(1, "Cheeseburger"),
        new Dish(1, "Veggie Burger"),
        new Dish(2, "Chicken Curry"),
        new Dish(2, "Beef Stroganoff"),
        new Dish(3, "Caesar Salad"),
        new Dish(3, "Greek Salad"),
        new Dish(4, "Margarita Pizza"),
        new Dish(4, "Pepperoni Pizza"),
        new Dish(5, "Sushi Rolls"),
        new Dish(5, "Sashimi")
    ];

    internal static readonly Service Service = new Service();
    internal static readonly PlaceStorage PlaceStorage = new PlaceStorage();
    internal static readonly CourierStorage CourierStorage = new CourierStorage();
    internal static readonly EaterStorage EaterStorage = new EaterStorage();

    internal static readonly List<Eater> Eaters =
    [
        new Eater(Names[0], PhoneNumbers[0], Service),
        new Eater(Names[1], PhoneNumbers[1], Service),
        new Eater(Names[2], PhoneNumbers[2], Service),
        new Eater(Names[3], PhoneNumbers[3], Service)
    ];

    internal static readonly List<Menu> Menus =
    [
        new Menu(1, Dishes.Where(d => d.RestaurantId == 1).ToList()),
        new Menu(2, Dishes.Where(d => d.RestaurantId == 2).ToList()),
        new Menu(3, Dishes.Where(d => d.RestaurantId == 3).ToList()),
        new Menu(4, Dishes.Where(d => d.RestaurantId == 4).ToList()),
        new Menu(5, Dishes.Where(d => d.RestaurantId == 5).ToList())
    ];

    internal static readonly List<Place> Places =
    [
        new Place(RestaurantNames[0], Addresses[0], Menus[0]),
        new Place(RestaurantNames[1], Addresses[1], Menus[1]),
        new Place(RestaurantNames[2], Addresses[2], Menus[2]),
        new Place(RestaurantNames[3], Addresses[3], Menus[3]),
    ];

    internal static readonly List<Order> Orders =
    [
        new Order(new List<Dish> { Dishes[0], Dishes[1] }, Addresses[0], Eaters[0]),
        new Order(new List<Dish> { Dishes[2], Dishes[4], Dishes[7] }, Addresses[1], Eaters[1]),
        new Order(new List<Dish> { Dishes[3], Dishes[4] }, Addresses[2], Eaters[2])
    ];

    internal static readonly List<Courier> Couriers =
    [
        new Courier(Names[0], Passports[0], Service),
        new Courier(Names[1], Passports[1], Service),
        new Courier(Names[2], Passports[2], Service),
        new Courier(Names[3], Passports[3], Service)
    ];


}