using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Database;
using ZeroHunger.DB;

namespace ZeroHunger.Controllers
{
	[Authorize]
	public class NGOController : Controller
	{
		// GET: NGO
		public ActionResult Index()
		{
			var db = new ZeroHungerEntities();
			return View(db.NGOs.SingleOrDefault());
		}



		[HttpGet]
		public ActionResult AddNewEmployee()
		{
			return View();
		}
		[HttpPost]
		public ActionResult AddNewEmployee(Employee cs)
		{
			var db = new ZeroHungerEntities();
			db.Employees.Add(new Employee()
			{
				Name = cs.Name,
				Location = cs.Location,
				TotalCompletedRequests = 0,
				NGOId = 1,
			});

			var ngo = db.NGOs.SingleOrDefault();
			ngo.TotalEmployees = ngo.TotalEmployees + 1;

			db.Accounts.Add(new Account()
			{
				Name = cs.Name,
				Password = Request["Password"],
				Role = "Employee",
			});

			db.SaveChanges();
			return RedirectToAction("Index");
		}



		public ActionResult EmployeeList()
		{
			var db = new ZeroHungerEntities();
			return View(db.Employees.ToList());
		}



		public ActionResult AssignTask()
		{
			var db = new ZeroHungerEntities();
			var Requests = (from r in db.CollectRequests
							where r.Status == "Requested"
							select r).ToList();
			return View(Requests);
		}



		public ActionResult AllTasks()
		{
			var db = new ZeroHungerEntities();
			var Requests = db.CollectRequests.ToList();
			return View(Requests);
		}



		[HttpGet]
		public ActionResult AssignEmployees(int id)
		{
			var db = new ZeroHungerEntities();
			Session["CollectReqId"] = id;
			return View(db.Employees.ToList());
		}
		[HttpPost]
		public ActionResult AssignEmployees(int[] Employees)
		{
			var db = new ZeroHungerEntities();
			var collectreqid = Int32.Parse(Session["collectreqid"].ToString());

			if (Employees.Length == 1)
			{
				var assigned = (from r in db.CollectRequests
								where r.Id == collectreqid
								select r).SingleOrDefault();
				assigned.EmployeeId = Employees[0];
				assigned.Status = "Assigned";
				db.SaveChanges();

				return RedirectToAction("AssignTask");
			}
			TempData["msg"] = "Can not assign more than 1 employee";
			return RedirectToAction("AssignEmployees");
		}
	}
}