﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
