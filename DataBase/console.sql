CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Таблица для сотрудников (Employee)
CREATE TABLE Employee (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    PhoneNumber VARCHAR(20),
    BirthDay DATE,
    Position VARCHAR(100),
    Contract TEXT,
    Salary DECIMAL(10, 2)
);

-- Таблица для клиентов (Client)
CREATE TABLE Client (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    PhoneNumber VARCHAR(20),
    BirthDay DATE,
    Passport VARCHAR(20),
    RegistrationDate DATE
);

-- Таблица для банковских счетов (Account) с внешним ключом
CREATE TABLE Account (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    ClientId UUID REFERENCES Client(Id) ON DELETE CASCADE,
    CurrencyName VARCHAR(50),
    Amount DECIMAL(10, 2)
);

-- Вставка данных в таблицу клиентов
INSERT INTO Client (FirstName, LastName, PhoneNumber, BirthDay, Passport, RegistrationDate)
VALUES ('John', 'Doe', '1234567890', '1985-05-15', 'A1234567', '2020-01-01'),
       ('Jane', 'Smith', '0987654321', '1990-07-25', 'B7654321', '2021-06-15');

-- Вставка данных в таблицу сотрудников
INSERT INTO Employee (FirstName, LastName, PhoneNumber, BirthDay, Position, Contract, Salary)
VALUES ('Alice', 'Brown', '2345678901', '1980-03-22', 'Manager', 'Full-Time', 5000),
       ('Bob', 'White', '3456789012', '1975-09-12', 'Consultant', 'Part-Time', 2500);

-- Вставка данных в таблицу счетов (удачная попытка, существующий клиент)
INSERT INTO Account (ClientId, CurrencyName, Amount)
VALUES ('25dbd9ae-bb42-46ea-a0c5-847255052afa', 'USD', 50000.00);

-- Неудачная попытка вставки данных (несуществующий клиент)
INSERT INTO Account (ClientId, CurrencyName, Amount)
VALUES ('00000000-0000-0000-0000-000000000000', 'USD', 500.00);

-- б) Выборка клиентов, у которых сумма на счету ниже определенного значения, отсортированных в порядке возрастания суммы
SELECT c.FirstName, c.LastName, a.Amount
FROM Client c
         JOIN Account a ON c.Id = a.ClientId
WHERE a.Amount < 5000
ORDER BY a.Amount ASC;

-- в) Поиск клиента с минимальной суммой на счете
SELECT c.FirstName, c.LastName, a.Amount
FROM Client c
         JOIN Account a ON c.Id = a.ClientId
ORDER BY a.Amount ASC
LIMIT 1;

-- г) Подсчет суммы денег у всех клиентов банка
SELECT SUM(a.Amount) AS TotalAmount
FROM Account a;

-- д) С помощью оператора JOIN, получить выборку банковских счетов и их владельцев
SELECT c.FirstName, c.LastName, a.CurrencyName, a.Amount
FROM Client c
         JOIN Account a ON c.Id = a.ClientId;

-- е) Вывести клиентов от самых старших к самым младшим
SELECT c.FirstName, c.LastName, c.BirthDay
FROM Client c
ORDER BY c.BirthDay ASC;

-- ж) Подсчитать количество человек, у которых одинаковый возраст
SELECT EXTRACT(YEAR FROM AGE(c.BirthDay)) AS Age, COUNT(*) AS NumberOfClients
FROM Client c
GROUP BY EXTRACT(YEAR FROM AGE(c.BirthDay));

-- з) Сгруппировать клиентов банка по возрасту
SELECT EXTRACT(YEAR FROM AGE(c.BirthDay)) AS Age, COUNT(*) AS NumberOfClients
FROM Client c
GROUP BY EXTRACT(YEAR FROM AGE(c.BirthDay))
ORDER BY Age ASC;

-- и) Вывести только N человек из таблицы
SELECT c.FirstName, c.LastName
FROM Client c
LIMIT 2;