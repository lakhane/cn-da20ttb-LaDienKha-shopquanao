using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDcustomerController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DsCustomer()
        {
            try
            {
                var dsCustomer = (from i in db.Customers
                                  orderby i.lastName ascending
                                  select new
                                  {
                                      customerId = i.customerId,
                                      firstName = i.firstName,
                                      lastName = i.lastName,
                                      email = i.email,
                                      phone = i.phone,
                                      address = i.address
                                  }).ToList();

                return Json(new { code = 200, dsCustomer = dsCustomer, msg = "Nhận thành công danh sách Khách hàng!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách Khách hàng sai: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult Delete(int customerId)
        {
            try
            {
                Customer customer = (from i in db.Customers
                                            select i).SingleOrDefault(model => model.customerId == customerId);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công!!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa không thành công! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}