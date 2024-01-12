using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDcategoryController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsCategory()
        {
            try
            {
                var dsCategory = (from i in db.ProductCategories
                                select new
                                {
                                   categoryId = i.categoryId,
                                   categoryName = i.categoryName
                                }).ToList();

                return Json(new { code = 200, dsCategory = dsCategory, msg = "Nhận danh sách Danh mục thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Nhận danh sách Danh mục sai: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult AddCategory(string categoryName)
        {
            try
            {
                var category = new ProductCategory();
                category.categoryName = categoryName;

                db.ProductCategories.Add(category);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Đã thêm thành công danh mục sản phẩm mới!!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi:" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int categoryId)
        {
            try
            {
                var detail = (from i in db.ProductCategories
                              select new
                              {
                                  categoryId = i.categoryId,
                                  categoryName = i.categoryName
                              }).SingleOrDefault(model => model.categoryId == categoryId);
                //var detail = db.ProductCategories.Where(model => model.categoryId == categoryId).SingleOrDefault();
                return Json(new { code = 200, detail = detail, msg = "Nhận thông tin chi tiết thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Không lấy được chi tiết!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int categoryId, string categoryName)
        {
            try
            {
                var category = (from i in db.ProductCategories
                                select i).SingleOrDefault(model => model.categoryId == categoryId);
                category.categoryName = (string)categoryName;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int categoryId)
        {
            try
            {
                ProductCategory category = (from i in db.ProductCategories
                                select i).SingleOrDefault(model => model.categoryId == categoryId);
                db.ProductCategories.Remove(category);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}