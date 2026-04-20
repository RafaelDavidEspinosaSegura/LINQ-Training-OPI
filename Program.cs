using LinqTraining.Data;
using LinqTraining.Models;

List <Product> products = SampleData.GetProducts(); 
List <Book> books = SampleData.GetBooks();
List<Order> orders = SampleData.GetOrders();
List<Employee> employees = SampleData.GetEmployees();

var employeesDictionary = employees.ToDictionary(e => e.Id);

Console.WriteLine($"Employee with ID 3: {employeesDictionary[3].Name}");

// pedidos del ultimo mes agrupado por estado
var lastMonth = orders
    .Where(o => o.Date >= DateTime.Today.AddMonths(-1))
    .GroupBy(o => o.Status)
    .Select(g => new { Estado = g.Key, Cantidad = g.Count() });

foreach (var grupo in lastMonth)
{
    Console.WriteLine($"{grupo.Estado}: {grupo.Cantidad}");
}
// top 5 libros mas vendidos por genero
var topBooks = books
    .GroupBy(b => b.Genre)
    .SelectMany(g => g
        .OrderByDescending(b => b.UnitsSold)
        .Take(5)
        .Select(b => new { Genero = g.Key, b.Title, b.UnitsSold }));

foreach (var libro in topBooks)
{
    Console.WriteLine($"{libro.Genero} - {libro.Title} ({libro.UnitsSold} vendidos)");
}
// Búsqueda de titulo de libro por nombre parcial

string search = "Code"; 
var foundBooks = books
    .Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

foreach (var libro in foundBooks)
{
    Console.WriteLine($"{libro.Title} - {libro.Author}");
}
// Top 3 clientes que mas compraron
var topClients = orders
    .GroupBy(o => o.CustomerName)
    .Select(g => new { Cliente = g.Key, TotalCompras = g.Sum(o => o.Amount) })
    .OrderByDescending(g => g.TotalCompras)
    .Take(3);

foreach (var cliente in topClients)
{
    Console.WriteLine($"{cliente.Cliente}: {cliente.TotalCompras:C}");
}
// clientes con ningún pedido completado

var noorder = orders
    .GroupBy(o => o.CustomerName)
    .Where(g => !g.Any(o => o.Status == "Completed"))
    .Select(g => g.Key);

foreach (var cliente in noorder)
{
    Console.WriteLine(cliente);
}


