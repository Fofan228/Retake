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
            operations = new List<Operations>();
        }
        public int Money { get; private set; }
        public int CurrentCount { get; private set; }

        private List<Operations> operations;
        public void OperationWithCount(Operations opertions)
        {
            operations.Add(opertions);
            switch (opertions.Operation)
            {
                case "in":
                    Money += opertions.Transaction;
                    CurrentCount = Money;
                    break;
                case "out":
                    if (Money > 0)
                    {
                        Money -= opertions.Transaction;
                        CurrentCount = Money;
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
                    Console.WriteLine(CurrentCount);
                    return;
                }
            }
            Console.WriteLine(Money);
        }
    }
    class Operations
    {
        public Operations(int transaction, DateTime date, string operation)
        {
            Transaction = transaction;
            Date = date;
            Operation = operation;
        }
        public Operations(DateTime date, string operation)
        {
            Date = date;
            Operation = operation;
        }
        public int Transaction { get; }
        public DateTime Date { get; }
        public string Operation { get; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.CurrentDirectory + "\\test.txt";
            string[] file = File.ReadAllLines(path);
            var count = new Count(int.Parse(file[0]));
            GoToCheckFile(file, count);
            count.EnterDate();
        }
        static void GoToCheckFile(string[] file, Count count)
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
                var operationWithRevert = new Operations(Convert.ToDateTime(correctStr[0].Trim()), correctStr[1].Trim());
                count.OperationWithCount(operationWithRevert);
            }
            else
            {
                var operation = new Operations(int.Parse(correctStr[1].Trim()), Convert.ToDateTime(correctStr[0].Trim()), correctStr[2].Trim());
                count.OperationWithCount(operation);
            }
        }
    }
}
