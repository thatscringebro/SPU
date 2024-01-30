using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SPU.Models;

namespace SPU.Controllers;

public class EvaluationController : Controller
{
    private readonly ILogger<EvaluationController> _logger;

    public EvaluationController(ILogger<EvaluationController> logger)
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
