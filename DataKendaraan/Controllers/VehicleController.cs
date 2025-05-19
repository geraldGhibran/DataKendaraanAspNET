using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataKendaraan.Models;

namespace DataKendaraan.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            DBModel db = new DBModel(); // No IDisposable error here
            List<Vehicle> list = db.Vehicles.ToList();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
                return View(new Vehicle());
            else 
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Vehicles.Where(x => x.Id==id).FirstOrDefault<Vehicle>());
                }
            }
        }

        [HttpGet]

        public ActionResult Detail(int id = 0)
        {
            if (id == 0)
                return View(new Vehicle());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Vehicles.Where(x => x.Id == id).FirstOrDefault<Vehicle>());
                }
            }
        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Vehicle vhc = db.Vehicles.Where(x => x.Id == id).FirstOrDefault<Vehicle>();
                db.Vehicles.Remove(vhc);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Vehicle vhc)
        {
            try
            {
                using (DBModel db = new DBModel())  // Use using statement for proper disposal
                {
                    if (vhc.Id == 0)
                    {
                        db.Vehicles.Add(vhc);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(vhc).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}