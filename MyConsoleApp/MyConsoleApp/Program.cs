Console.WriteLine("Hello, World!");
var date = DateTime.Now;
var daysToChristmas = (new DateTime(DateTime.Now.Year, 12, 25) - DateTime.Now).Days;
Console.WriteLine($"The current date and time is: {date}");
Console.WriteLine($"There are {daysToChristmas} days left until Christmas.");