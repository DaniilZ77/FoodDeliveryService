# oop2-2023-class4

Напишите код для сервиса доставки еды. Базовые классы уже описаны. Допишите их и наполните логикой.

1. Основные сущности для сервиса доставки еды - рестораны, курьеры и пользователи. Подумайте какие данные нужны в этих классах для выполнения заказов и расширьте существующие классы. 
2. Данные о ресторанах, курьерах и пользователях мало описать, их нужно где-то хранить. Реализуйте класс Storage, который будет хранить данные о них, давать доступ к данным по идентификатору и давать возможность изменять данные. Очевидно что Storage это в большинстве случаев внешний сервис - помните про DIP и CQRS
3. Теперь пришло время реализовать логику. Пусть логика будет описана в классе Service, который будет использовать хранилище и предоставлять интерфейс для пользователя, ресторана и курьера. Вероятно для выполнения все логики требуется доработка уже существующей системы классов.
3.1 - Пользователь должен иметь возможность получить список ресторанов
3.2 - Пользователь должен иметь возможность получить меню конкретного ресторана
3.3 - Пользователь должен иметь возможность разместить заказ
3.4 - Ресторан должен иметь возможность получить список активных своих заказов
3.5 - Ресторан должен иметь возможность получить список блюд конкретного заказа
3.6 - Ресторан должен иметь возможность менять статус заказа
3.7 - Курьер должен иметь возможность получить список готовых заказов, на которых не назначены курьеры
3.8 - Курьер дрлжен иметь возможность менять статусы заказа
4. Напишите тесты для всех методов и классов, которые были реализованы.
