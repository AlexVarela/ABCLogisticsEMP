using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABCLogisticsEMP.Models;

namespace ABCLogisticsEMP.Controllers
{
    public class EmployeesController : Controller
    {
        private ABCLogisticsEMP_dbEntities db = new ABCLogisticsEMP_dbEntities();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).Include(e => e.Level);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name");
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name");
            
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "fake"), "Id", "Name");
            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastName,SSN,Address,Phone,Email,Title,Levels_Id,Departments_Id,Employee1_Id")] Employee employee)
        {
            ViewBag.ErrorMessage = "";
            bool isValid = true;
            if (employee.Levels_Id != 5)
            {
                if (employee.Employee1_Id == null || employee.Employee1_Id == 0)
                {
                    isValid = false;
                    ViewBag.ErrorMessage = "Non supervisors must have one";
                } 
            }

            if (ModelState.IsValid && isValid)
            {
               

                Department dept = db.Departments.Find(employee.Departments_Id);
                Level lev = db.Levels.Find(employee.Levels_Id);

                employee.Title = dept.Name + " " + lev.Name + " Associate";
                if (employee.Employee1_Id == 0)
                {
                    employee.Employee1_Id = null;
                }

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name", employee.Departments_Id);
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name", employee.Levels_Id);
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "Supervisor" && x.Departments_Id == employee.Departments_Id), "Id", "Name");
            
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name", employee.Departments_Id);
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name", employee.Levels_Id);
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Id == employee.Employee1_Id), "Id", "Name");
            ViewBag.ErrorMessage = "";
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,SSN,Address,Phone,Email,Title,Levels_Id,Departments_Id,Employee1_Id")] Employee employee)
        {
            ViewBag.ErrorMessage = "";
            bool isValid = true;
            if (employee.Levels_Id != 5)
            {
                if (employee.Employee1_Id == null || employee.Employee1_Id == 0)
                {
                    isValid = false;
                    ViewBag.ErrorMessage = "Non supervisors must have one";
                }
            }

            if (ModelState.IsValid && isValid)
            {

                if (employee.Employee1_Id == 0)
                {
                    employee.Employee1_Id = null;
                }

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name", employee.Departments_Id);
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name", employee.Levels_Id);
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "Supervisor" && x.Departments_Id == employee.Departments_Id), "Id", "Name");
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);

            List<Bank> bankList = employee.Banks.ToList();

            foreach (Bank bid in bankList)
            {

                db.Banks.Remove(bid);
                
            }


            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Action result for ajax call
        [HttpPost]
        public ActionResult GetSupervisorByDepartment(int departmentid)
        {
            var Supervisors =  db.Employees.Where(x => x.Level.Name == "Supervisor" && x.Departments_Id == departmentid).Select(x => new
            {
                Value = x.Id,
                Text = x.Name + " " + x.LastName
            }).ToList();

            return Json(Supervisors);

        }
    }
}
