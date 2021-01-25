using QulixApp.Domain;
using QulixApp.HtmlAttribute;
using QulixApp.Models;
using System.Collections.Generic;
using System.Web.Mvc;



namespace QulixApp.Controllers
{
    public class EmployeeController:Controller
    {
        
        public int pageSize = 10;
        public int showPages = 15;
        public int count = 0;

        // отображение списка пользователей
        public ViewResult Index(string sortOrder, int page = 1)
        {
            string sortName = null;
            System.Web.Helpers.SortDirection sortDir = System.Web.Helpers.SortDirection.Ascending;
            sortOrder = Base.parseSortForDB(sortOrder, out sortName, out sortDir);
            EmployeeRepository rep = new EmployeeRepository();
            EmployeeGrid employee = new EmployeeGrid
            {
                Employee = rep.List(sortName, sortDir, page, pageSize, out count),
                PagingInfo = new PagingInfo
                {
                    currentPage = page,
                    itemsPerPage = pageSize,
                    totalItems = count,
                    showPages = showPages
                },
                SortingInfo = new SortingInfo
                {
                    currentOrder = sortName,
                    currentDirection = sortDir
                }
            };
            return View(employee);
            
        }

        [ReferrerHold]
        [HttpPost]
        public ActionResult Index(string onNewUser)
        {
            if (onNewUser != null)
            {
                
                TempData["referrer"] = ControllerContext.RouteData.Values["referrer"];
                return View("New", new EmployeeModel(new EmployeeClass(), Companies()));
            }
            return View();
        }

        [ReferrerHold]
        public ActionResult New()
        {
            TempData["referrer"] = ControllerContext.RouteData.Values["referrer"];
            return View("New", new EmployeeModel(new EmployeeClass(), Companies()));
        }

        [HttpPost]
        public ActionResult New(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Employee == null || model.Employee.Company == null || model.Employee.Company.CompanyID == 0) RedirectToAction("Index");
                EmployeeRepository rep = new EmployeeRepository();
                if (rep.AddEmployee(model.Employee)) TempData["message"] = string.Format("{0} has been added", model.Employee.Surname);
                else TempData["error"] = string.Format("{0} has not been added!", model.Employee.Surname);
                if (TempData["referrer"] != null) return Redirect(TempData["referrer"].ToString());
                return RedirectToAction("Index");
            }
            else
            {
                model = new EmployeeModel(model.Employee, Companies()); // почему-то при невалидной модели в данный метод приходит пустой список model.Company, приходится перезаполнять
                return View(model);
            }
        }

        [ReferrerHold]
        public ActionResult Edit(int UserID)
        {
            EmployeeRepository rep = new EmployeeRepository();
            EmployeeClass user = rep.FetchByID(UserID);
            if (user == null) return HttpNotFound();
            TempData["referrer"] = ControllerContext.RouteData.Values["referrer"];
            return View(new EmployeeModel(user, Companies()));
        }

        [HttpPost]
        public ActionResult Edit(EmployeeModel model, string action)
        {
            if (action == "Cancel")
            {
                if (TempData["referrer"] != null) return Redirect(TempData["referrer"].ToString());
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                if (model.Employee == null || model.Employee.Company == null || model.Employee.Company.CompanyID == 0) RedirectToAction("Index");
                EmployeeRepository rep = new EmployeeRepository();
                if (action == "Save")
                {
                    if (rep.ChangeEmployee(model.Employee)) TempData["message"] = string.Format("{0} has been saved", model.Employee.Surname);
                    else TempData["error"] = string.Format("{0} has not been saved!", model.Employee.Surname);
                }
                if (action == "Remove")
                {
                    if (rep.RemoveEmployee(model.Employee)) TempData["message"] = string.Format("{0} has been removed", model.Employee.Surname);
                    else TempData["error"] = string.Format("{0} has not been removed!", model.Employee.Surname);
                }
                if (TempData["referrer"] != null) return Redirect(TempData["referrer"].ToString());
                return RedirectToAction("Index");
            }
            else
            {
                model = new EmployeeModel(model.Employee, Companies());
                return View(model);
            }
        }

        public IList<CompanyClass> Companies()
        {
            CompanyRepository rep = new CompanyRepository();
            return rep.List();
        }
    }
}