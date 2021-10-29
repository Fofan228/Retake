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
        }
        public int Money { get; private set; }
        public List<Opertions> Operations { get; set; }
        public void CorrectCount(Count count, Opertions opertions)
        {
            switch (opertions.Operation)
            {
                case "in":
                    count.Money += opertions.Transaction;
                    opertions.GetMoney(count.Money);
                    break;
                case "out":
                    if (count.Money > 0)
                    {
                        count.Money -= opertions.Transaction;
                        opertions.GetMoney(count.Money);
                    }
                    else throw new Exception("Некорректный файл. Баланс ниже нуля!");
                    break;
                case "revert":
                    throw new Exception("Отмена операции");
            }
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
            EnterDate(CreateBank(file));
        }
        static Count CreateBank(string[] file)
        {
            var count = new Count(int.Parse(file[0]));
            count.Operations = new List<Opertions>();
            for (int i = 1; i < file.Length; i++)
            {
                CheckString(file[i], count);
            }
            return count;
        }
        static void CheckString(string str, Count count)
        {
            string[] correctStr = str.Split('|');
            var operation = new Opertions(int.Parse(correctStr[1].Trim()), Convert.ToDateTime(correctStr[0].Trim()), correctStr[2].Trim());
            count.CorrectCount(count, operation);
            count.Operations.Add(operation);
        }
        static void EnterDate(Count count)
        {
            Console.WriteLine("Введите дату и время операции:");
            string str = Console.ReadLine();
            foreach (var item in count.Operations)
            {
                if (str == Convert.ToString(item.Date))
                {
                    Console.WriteLine(item.MoneyInTransaction);
                    return;
                }
            }
            Console.WriteLine(count.Money);
        }
    }
}
