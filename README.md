# TestTaskFinbridge

1-3 Задание выполнено в проекте Task1-3, 4-5 в Task4-5.

Задание разбито на несколько уровней. Можно остановиться на любом из уровней и не выполнять всё задание целиком.  Конечно, чем больше уровней сделано, тем лучше. Но главным является качество кода. Желательно исходники загрузить на публичную систему контроля версий (Github.com и другие) с развёрнутой историей коммитов. Плюсом будет наличие unit-тестов. Для реализации тестового задания использовать ASP.NET 7.

Уровень 1. Базовый
Реализовать REST веб-сервис по расчёту сумм квадратов чисел переданной последовательности (x1, x2, ..., xn) => x1^2 + x2^2 + ... xn^2. Максимальное и минимальное возможные значения xi, максимальное возможное количество аргументов N задаются в конфигурационном файле веб-сервиса. 
Если количество аргументов или значение одного из них превосходят установленные ограничение, то веб-сервис отвечает ошибкой ввода данных.
Для передачи входящих и исходящих данных используется формат json. Входящая последовательность передаётся как массив.
Например: POST calculate
{
    "values": [ 1, 50, 40 ]
}

Уровень 2. Добавление ожидания
Операция расчёта квадрата (xi^2) числа становится "ресурсозатратной" и занимает некоторое время. (Именно операция расчёта квадрата одного числа, а не всё вычисление целиком) В программном коде это эмулируется ожиданием случайного промежутка времени, максимальное и минимальные границы которого тоже задаются в конфигурации веб-сервиса.

Уровень 3. Кэширование
Для ускорения работы сервиса результаты операции вычисления квадрата числа (xi^2) кэшируются

Уровень 4. Сайт
Реализовать веб-сайт (использовать bootstrap, адаптивный дизайн) - клиента веб-сервиса реализованного ранее. Сайт состоит из единственной страницы, содержит поля ввода данных, кнопку выполнить и поле вывода результата. Вычисление и вывод результата должны происходить без перезагрузки страницы. 

Уровень 5. История на сайте
Показывать на странице историю полученных результатов (можно без сохранения, т.е. перезагрузка страницы очищает историю).

