using System;
using System.Collections.Generic;
using System.IO;

namespace ZAEBAL
{
    class Count
    {
        public Count(int money)
        {
            Money = money;
            operations = new List<Opertions>();
        }
        public int Money { get; private set; }

        private List<Opertions> operations;
        public void CorrectCount(Opertions opertions)
        {
            operations.Add(opertions);
            switch (opertions.Operation)
            {
                case "in":
                    Money += opertions.Transaction;
                    opertions.GetMoney(Money);
                    break;
                case "out":
                    if (Money > 0)
                    {
                        Money -= opertions.Transaction;
                        opertions.GetMoney(Money);
                    }
                    else throw new Exception("Баланс ниже нуля!");
                    break;
                case "revert":
                    if (operations[operations.Count - 2].Operation == "out")
                        Money += operations[operations.Count - 2].Transaction;
                    else if (operations[operations.Count - 2].Operation == "in")
                        Money -= operations[operations.Count - 2].Transaction;
                    break;
            }
        }
        public void EnterDate()
        {
            Console.WriteLine("Введите дату и время операции:");
            string str = Console.ReadLine();
            foreach (var item in operations)
            {
                if (str == Convert.ToString(item.Date))
                {
                    Console.WriteLine(item.MoneyInTransaction);
                    return;
                }
            }
            Console.WriteLine(Money);
        }
    }
    class Opertions
    {
        public Opertions(int transaction, DateTime date, string operation)
        {
            Transaction = transaction;
            Date = date;
            Operation = operation;
        }
        public Opertions(DateTime date, string operation)
        {
            Date = date;
            Operation = operation;
        }
        public int MoneyInTransaction { get; private set; }
        public int Transaction { get; }
        public DateTime Date { get; }
        public string Operation { get; }
        public void GetMoney(int money)
        {
            MoneyInTransaction = money;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.CurrentDirectory + "\\test.txt";
            string[] file = File.ReadAllLines(path);
            var count = new Count(int.Parse(file[0]));
            CreateBank(file, count);
            count.EnterDate();
        }
        static void CreateBank(string[] file, Count count)
        {
            for (int i = 1; i < file.Length; i++)
            {
                CheckString(file[i], count);
            }
        }
        static void CheckString(string str, Count count)
        {
            string[] correctStr = str.Split('|');
            if (correctStr.Length == 2)
            {
                var operationRevert = new Opertions(Convert.ToDateTime(correctStr[0].Trim()), correctStr[1].Trim());
                count.CorrectCount(operationRevert);
            }
            else
            {
                var operation = new Opertions(int.Parse(correctStr[1].Trim()), Convert.ToDateTime(correctStr[0].Trim()), correctStr[2].Trim());
                count.CorrectCount(operation);
            }
        }
    }
}
