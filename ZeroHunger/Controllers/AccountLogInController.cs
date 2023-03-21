using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using ZeroHunger.Database;


namespace ZeroHunger.Controllers
{
	public class AccountLogInController : Controller
	{
		// GET: AccountLogIn
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Index(AccountLogInTable a)
		{
			var db = new ZeroHungerEntities();
			db.AccountLogInTables.Add(new AccountLogInTable()
			{
				AccountLogName = a.AccountLogName,
				AccountLogPassword = a.AccountLogPassword,
		
			});
			db.SaveChanges();

			db.RestaurantTables.Add(new RestaurantTable()
			{
				RestName = a.RestName,
				RestLocation = Request["Location"],
			}); ;
			db.SaveChanges();
			return RedirectToAction("Login");
		}
		[HttpGet]
		public ActionResult Login()
		{
			var db = new ZeroHungerEntities();
			var allCollectRequests = db.CollectRequests.ToList();

			foreach (var item in allCollectRequests)
			{
				if (((TimeSpan)(DateTime.Now - item.PlacingDate)).Days > 3 && item.Status != "Completed")
				{
					item.Status = "Wasted";
				}
			}
			db.SaveChanges();
			return View();
		}
		[HttpPost]
		public ActionResult Login(Account a)
		{
			var db = new ZeroHungerEntities();

			var allCollectRequests = db.CollectRequests.ToList();

			foreach (var item in allCollectRequests)
			{
				if (((TimeSpan)(DateTime.Now - item.PlacingDate)).Days > 3 && item.Status != "Completed")
				{
					item.Status = "Wasted";
				}
			}
			db.SaveChanges();

			var user = (from u in db.Accounts
						where u.Name == a.Name && u.Password == a.Password
						select u).SingleOrDefault();

			if (user != null)
			{
				FormsAuthentication.SetAuthCookie((user.Id).ToString(), true);

				if (user.Role == "Restaurant")
				{
					var AllRestaurants = db.Restaurants.ToList();

					foreach (var item in AllRestaurants)
					{
						if (item.Name == user.Name)
						{
							return RedirectToAction("AllCollectRequest", "Restaurant", new { id = item.Id });
						}
					}
				}
				else if (user.Role == "Employee")
				{
					var AllEmployees = db.Employees.ToList();

					foreach (var item in AllEmployees)
					{
						if (item.Name == user.Name)
						{
							return RedirectToAction("ShowAssignedTasks", "Employee", new { id = item.Id });
						}
					}
				}
				else if (user.Role == "NGO")
				{
					var AllNGO = db.NGOs.SingleOrDefault();

					if (AllNGO.Name == user.Name)
					{
						return RedirectToAction("Index", "NGO", new { id = AllNGO.Id });
					}
				}
			}
			return RedirectToAction("Login");
		}



		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Login");
		}
	}
}