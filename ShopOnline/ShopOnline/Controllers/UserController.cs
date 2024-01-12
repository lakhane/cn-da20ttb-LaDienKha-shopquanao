using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using CaptchaMvc.HtmlHelpers;

namespace ShopOnline.Controllers
{
    public class UserController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["info"] != null) // Nếu đã đăng nhập rồi thì sửa link vào đăng nhập sẽ điều hướng sang trang chủ
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tk = collection["username"];
            var mk = collection["password"];
            mk = Encryptor.MD5Hash(mk);

            var check = db.Members.SingleOrDefault(model => model.userName == tk && model.password == mk);
            if (ModelState.IsValid)
            {
                if (check == null)
                {
                    ModelState.AddModelError("", "Đã xảy ra sự cố khi đăng nhập. Hãy kiểm tra tên người dùng và mật khẩu của bạn hoặc tạo một tài khoản.");
                }
                else
                {
                    if (!this.IsCaptchaValid(""))
                    {
                        ViewBag.captcha = "Captcha xác thực không hợp lệ";
                    }
                    else
                    {
                        Session["info"] = check;
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = db.Members.Where(model => model.userName == member.userName).FirstOrDefault();

                    if (check != null)
                    {
                        // check username constained in database
                        ModelState.AddModelError("", "Đã xảy ra sự cố khi tạo tài khoản của bạn. Tên người dùng của bạn đã tồn tại.");
                        return View(member);
                    }
                    else
                    {
                        member.password = Encryptor.MD5Hash(member.password);
                        member.dateOfJoin = DateTime.Now;
                        member.roleId = 3;
                        member.avatar = "~/Content/img/avatar/avatar.jpg";
                        member.status = true;
                        db.Members.Add(member);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            TempData["msgSuccess"] = "Tạo tài khoản thành công!";
                            return RedirectToAction("Login");
                        }
                    }
                }
                return View(member);
            }
            catch(Exception ex)
            {
                TempData["msgFailed"] = "Tạo tài khoản không thành công! " + ex.Message;
                return RedirectToAction("Login");
            }
        }
        public ActionResult logout()
        {
            Session.Remove("info");
            //Session["info"] = null;
            //Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}