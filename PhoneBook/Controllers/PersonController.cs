using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Helpers;
using PhoneBook.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhoneBook.Controllers
{
	public class PersonController : Controller
	{
		// GET: /<controller>/
		[HttpGet]
		public IActionResult Index(int id = 1)
		{
			ViewBag.ID = id;
			return View(SourceManager.Get(id, 5));
		}
		[HttpPost]
		public IActionResult Index(string what, string where="LastName")
		{
			return View(SourceManager.Search(what, where));
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Add(PersonModel personModel)
		{
			if (ModelState.IsValid)
			{
				var id = SourceManager.Add(personModel);
				return View("Succes");
			}
			return View(personModel);
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var getOne = SourceManager.GetById(id);
			return View(getOne);

		}
		[HttpPost]
		public IActionResult Edit(PersonModel personModel)
		{
			if (ModelState.IsValid)
			{
				SourceManager.Update(personModel);
				return View("Succes");
			}
			return View(personModel);
		}

		[HttpGet]
		public IActionResult Remove(int id)
		{
			var getOne = SourceManager.GetById(id);
			return View(getOne);
		}
		[HttpPost]
		public IActionResult RemoveConfirm(int id)
		{
			SourceManager.Remove(id);
			TempData["Success"] = "Succesfuly deleted the entry";
			return View("Succes");
		}


	}

	//TODO slajd 11 zmiana routingu

}
