using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultiThreadingLINQEvents
{
    // Multi-threading
    public class NumberProcessor
    {
        public void ProcessNumber(int number)
        {
            // Simulating time-consuming operation
            Thread.Sleep(new Random().Next(1000, 5000)); // Sleep for a random amount of time (between 1 and 5 seconds)
            Console.WriteLine($"Processed number: {number}");
        }
    }

    // LINQ
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Multi-threading
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            NumberProcessor numberProcessor = new NumberProcessor();
            foreach (int number in numbers)
            {
                // Create a new thread for each number and call ProcessNumber method on each thread
                Thread thread = new Thread(() => numberProcessor.ProcessNumber(number));
                thread.Start();
            }

            // LINQ
            List<Product> products = new List<Product>
            {
                new Product { Name = "Product1", Price = 10.5 },
                new Product { Name = "Product2", Price = 20.7 },
                new Product { Name = "Product3", Price = 30.2 }
            };

            double minPrice = 15.0;
            // Find products with price greater than or equal to specified value and convert their names to uppercase
            var filteredProducts = products.Where(p => p.Price >= minPrice)
                                           .Select(p => p.Name.ToUpper());

            foreach (string productName in filteredProducts)
            {
                Console.WriteLine($"Product with price greater than or equal to {minPrice}: {productName}");
            }

            // Events
            Downloader downloader = new Downloader();
            downloader.ProgressChanged += OnProgressChanged; // Subscribe to ProgressChanged event
            downloader.DownloadFile(); // Start downloading the file
        }

        // Events
        public delegate void ProgressUpdate(int progressPercentage);

        public class Downloader
        {
            public event ProgressUpdate ProgressChanged; // Define ProgressChanged event
            private int progress;

            public int Progress
            {
                get { return progress; }
                set
                {
                    progress = value;
                    OnProgressChanged(progress); // Raise ProgressChanged event when progress changes
                }
            }

            protected virtual void OnProgressChanged(int progress)
            {
                ProgressChanged?.Invoke(progress); // Invoke ProgressChanged event
            }

            public void DownloadFile()
            {
                for (int i = 0; i <= 100; i += 10)
                {
                    Thread.Sleep(1000); // Simulate download progress
                    Progress = i; // Update progress
                }
            }
        }

        public static void OnProgressChanged(int progressPercentage)
        {
            Console.WriteLine($"Download progress: {progressPercentage}%"); // Print progress percentage
        }
    }
}
