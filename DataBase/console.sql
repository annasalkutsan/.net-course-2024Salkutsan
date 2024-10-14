-- Таблица для должностей сотрудников (position)
CREATE TABLE position (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    position_name VARCHAR(100)
);

-- Таблица для сотрудников (employee)
CREATE TABLE employee (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    phone_number VARCHAR(20) UNIQUE,
    birth_day DATE,
    position_id UUID REFERENCES position(id),
    contract TEXT,
    salary DECIMAL(10, 2)
);

-- Таблица для клиентов (client)
CREATE TABLE client (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    phone_number VARCHAR(20),
    birth_day DATE,
    passport VARCHAR(20) UNIQUE,
    registration_date DATE
);

-- Таблица для банковских счетов (account)
CREATE TABLE account (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    client_id UUID REFERENCES client(id) ON DELETE CASCADE,
    currency_name VARCHAR(50),
    amount DECIMAL(10, 2)
);

-- Вставка данных в таблицу должностей
INSERT INTO position (position_name)
VALUES ('Manager'), ('Consultant');

-- Вставка данных в таблицу клиентов
INSERT INTO client (first_name, last_name, phone_number, birth_day, passport, registration_date)
VALUES ('John', 'Doe', '1234567890', '1985-05-15', 'A1234567', '2020-01-01'),
       ('Jane', 'Smith', '0987654321', '1990-07-25', 'B7654321', '2021-06-15');

-- Вставка данных в таблицу сотрудников
INSERT INTO employee (first_name, last_name, phone_number, birth_day, position_id, contract, salary)
VALUES ('Alice', 'Brown', '2345678901', '1980-03-22',
        (SELECT id FROM position WHERE position_name = 'Manager'), 'Full-Time', 5000),
       ('Bob', 'White', '3456789012', '1975-09-12',
        (SELECT id FROM position WHERE position_name = 'Consultant'), 'Part-Time', 2500);

-- Вставка данных в таблицу счетов (удачная попытка, существующий клиент)
INSERT INTO account (client_id, currency_name, amount)
VALUES ('d3b05c35-6304-4ca3-a89b-d7b9be15d742', 'USD', 50000.00);

-- Неудачная попытка вставки данных (несуществующий клиент)
INSERT INTO account (client_id, currency_name, amount)
VALUES ('00000000-0000-0000-0000-000000000000', 'USD', 500.00);

-- Выборка клиентов с суммой на счету ниже определенного значения, сортировка по возрастанию суммы
SELECT c.first_name, c.last_name, a.amount
FROM client c
         JOIN account a ON c.id = a.client_id
WHERE a.amount < 5000
ORDER BY a.amount ASC;

-- Поиск клиентов с минимальной суммой на счете
SELECT c.first_name, c.last_name, a.Amount
FROM Client c
         JOIN Account a ON c.Id = a.client_id
WHERE a.Amount = (SELECT MIN(Amount) FROM Account);

-- Подсчет общей суммы денег у всех клиентов банка
SELECT SUM(a.amount) AS total_amount
FROM account a;

-- Получение выборки банковских счетов и их владельцев с использованием JOIN
SELECT c.first_name, c.last_name, a.currency_name, a.amount
FROM client c
    JOIN account a ON c.id = a.client_id;

-- Вывести клиентов от самых старших к самым младшим
SELECT c.first_name, c.last_name, c.birth_day
FROM client c
ORDER BY c.birth_day ASC;

-- Подсчет количества клиентов по возрасту с использованием алиаса
SELECT EXTRACT(YEAR FROM AGE(c.birth_day)) AS age, COUNT(*) AS number_of_clients
FROM client c
GROUP BY EXTRACT(YEAR FROM AGE(c.birth_day))
ORDER BY age;

-- Сгруппировать клиентов по возрасту, отсортировать по возрастанию возраста
SELECT EXTRACT(YEAR FROM AGE(c.birth_day)) AS age, COUNT(*) AS number_of_clients
FROM client c
GROUP BY age
ORDER BY age ASC;

-- Вывести первых N клиентов из таблицы
SELECT c.first_name, c.last_name
FROM client c
    LIMIT 2;