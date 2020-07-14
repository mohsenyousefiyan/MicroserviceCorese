﻿using RabbitMQ.Client;
using Rbt.Core.Entities;
using System;
using Rbt.Toolkits;
using RabbitMQ.Client.Events;

namespace Publisher
{
    class Program
    {
        public static ConnectionFactory _connectionFactory;
        public static IConnection _connection;
        public static IModel _model;
        public const string QueueName = "TestQueue";

        static void Main(string[] args)
        {
            CreateConnection();

            string key;
            do
            {

                SendPayments(10);
                Console.WriteLine("".PadLeft(100, '-'));
                Console.WriteLine();

                Console.WriteLine("do you want to send message:y/any thing else ");
                key = Console.ReadLine();
                Console.WriteLine();

            } while (key.ToLower() == "y");


        }

        private static void SendPayments(int paymentCount)
        {
            for (int i = 0; i < paymentCount; i++)
            {
                Payment payment = new Payment
                {
                    FirstName = $"FiratName {i}",
                    LastName = $"LastName {i}",
                    CardNumber = $"1111-2222-3333-{i.ToString().PadLeft(4, '0')}",
                    Value = Math.Pow(i / 3, 5)
                };
                _model.BasicPublish("", QueueName, null, payment.ToByteArray());
                Console.WriteLine($"Message Send for {i}");
            }
        }

        public static void CreateConnection()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "10.100.8.67",
                UserName = "admin",
                Password = "admin",
                Port = Protocols.DefaultProtocol.DefaultPort
            };
            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(QueueName, true, false, false, null);
        }
    }
}