using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication5.Models;

namespace WebApplication5.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string connectionString = "server=localhost;database=test;uid=postgres;pwd=Test123!";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Submit(Order order)
    {
        string sql = "INSERT INTO orders (id, name, numberOfProducts) VALUES (@id, @name, @numberOfProducts)";

        using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@id", order.Id);
            cmd.Parameters.AddWithValue("@name", order.Name);
            cmd.Parameters.AddWithValue("@numberOfProducts", order.NumberOfProducts);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        return Redirect("/Home/Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}