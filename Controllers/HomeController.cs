using Microsoft.AspNetCore.Mvc;
using MVC_Site.Models;
using System.Diagnostics;

namespace MVC_Site.Controllers;
public class HomeController : Controller {
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger) {
		_logger = logger;
	}

	public IActionResult Index() => View();
		
	public IActionResult Resume() => View();
	
	public IActionResult Portfolio() => View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() {
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
