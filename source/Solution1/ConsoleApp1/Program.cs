using System;
using Domain.Position;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var position = new Position(
                id: 1,
                name: "Developer",
                descreption: "Backend developer",
                lifeTime: DateTime.UtcNow
            );
            
            Console.WriteLine($"Position: {position.Name}");
            Console.WriteLine($"Description: {position.Descreption}");
        }
    }
}







