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
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "Supervisor"), "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastName,SSN,Address,Phone,Email,Title,Levels_Id,Departments_Id,Employee1_Id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
               

                Department dept = db.Departments.Find(employee.Departments_Id);
                Level lev = db.Levels.Find(employee.Levels_Id);

                employee.Title = dept.Name + " " + lev.Name + " Associate";

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name", employee.Departments_Id);
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name", employee.Levels_Id);
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "Supervisor"), "Id", "Name");
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
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,SSN,Address,Phone,Email,Title,Levels_Id,Departments_Id,Employee1_Id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments_Id = new SelectList(db.Departments, "Id", "Name", employee.Departments_Id);
            ViewBag.Levels_Id = new SelectList(db.Levels, "Id", "Name", employee.Levels_Id);
            ViewBag.Supervisor_Id = new SelectList(db.Employees.Where(x => x.Level.Name == "Supervisor"), "Id", "Name");
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

            foreach (Bank bid in employee.Banks)
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
    }
}
