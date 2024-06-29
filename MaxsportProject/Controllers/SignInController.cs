//using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Ecommerce_KTPM.Controllers
{
    public class SignInController : Controller
    {
		// GET: SignIn
		//EcommerceEntities db = new EcommerceEntities();
		// Kiểm tra xem có phải người dùng thực hiện lệnh Update không
		public ActionResult Login()
        {
			var tb= TempData["TBDangNhap"] as string;
			ViewBag.mess = tb;
            return View();
        }
		[HttpPost]
		public ActionResult Login(string name, string pass)
		{
            //if (name != null)
            //	pass = CryptoService.ComputeSha1Hash(pass);
            //	foreach (TaiKhoanTV i in DataIn.GetTaiKhoanTVs().ToList())
            //	{
            //		if (i.TKTV.Equals(name.ToLower().Trim()) && i.matKhau.Equals(pass))
            //		{
            //			if (i.trangThai == true)
            //			{
            //				Session["DangNhap"] = i;
            //				if (i.TKTV == "admin" || i.ghiChu=="QTV") return RedirectToAction("DSTK", "QTV", new { Area = "PrivateShop" });
            //				if (Request.Cookies["returnUrl"] != null)
            //				{
            //					string Url = Request.Cookies["returnUrl"].Value;
            //					// Xóa cookie có tên "returnUrl"
            //					HttpCookie returnUrl = Request.Cookies["returnUrl"];

            //					if (returnUrl != null)
            //					{
            //						returnUrl.Expires = DateTime.Now.AddDays(-1);
            //						Response.Cookies.Add(returnUrl);
            //					}
            //					Response.Redirect(Url);
            //				}
            //				else return RedirectToAction("TrangThai", "TrangThai", new { Area = "PrivateShop" });
            //		}
            //			else
            //			{
            //				ViewBag.mess = "Tài khoản của bạn đã bị khoá.";
            //			}
            //		}
            //		else
            //		{
            //			ViewBag.mess = "Tên tài khoản hoặc mật khẩu nhập không đúng";
            //		}
            //	}
            //return View();
            RedirectToAction("TrangThai", "TrangThai", new { Area = "PrivateShop" });
        }
		
		public ActionResult Logout()
        {
			Session.Abandon();//remove session
			return RedirectToAction("Login");
        }
		public ActionResult SignUp() { return View(); }
		//[HttpPost]
		//public ActionResult SignUp(TaiKhoanTV x, string xnmk)
		//{
		//	using (DbContextTransaction trans = db.Database.BeginTransaction())
		//	{
		//		try
		//		{
		//			if (db.TaiKhoanTVs.Find(x.TKTV) == null)
		//			{
		//				if (x.matKhau.Equals(xnmk))
		//				{
		//					if (x.email == null) x.email = "";
		//					if (x.hoDem == null) x.hoDem = "";
		//					if (x.diaChi == null) x.diaChi = "";
		//					if (x.soDT == null) x.soDT = "";
		//					if (x.ghiChu == null) x.ghiChu = "";
		//					if (x.ngaysinh == null) x.ngaysinh = DateTime.Now;
		//					if (x.gioiTinh == null) x.gioiTinh = true;
		//					x.matKhau = CryptoService.ComputeSha1Hash(x.matKhau);
		//					x.TKTV = x.TKTV.ToLower().Trim();
		//					x.hinhDD = "";
		//					x.trangThai = true;
		//					db.TaiKhoanTVs.Add(x);
		//				}
		//				else
		//				{
		//					ViewBag.usc = "2 mật khẩu nhập khác nhau";
		//					return View("SignUp", x);
		//				}
		//			}
		//			else
		//			{
		//				ViewBag.usc = "Username đã tồn tại";
		//				return View("SignUp", x);
		//			}

		//			// lưu vào cơ sở dữ liệu
		//			db.SaveChanges();
		//			trans.Commit();
		//			ViewBag.sc = "Tạo tài khoản thành công";
		//			return View("Login");
		//		}
		//		catch (Exception ex)
		//		{
		//			trans.Rollback();
		//			string e = ex.Message;
		//		}
				
		//	}
		//	ViewBag.sc = "Tạo tài khoản không thành công";
		//	return View("SignUp",x);
		//}

	}
}