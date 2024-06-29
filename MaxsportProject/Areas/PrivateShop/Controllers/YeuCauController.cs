using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class YeuCauController : BaoMatController
    {
        // GET: PrivateShop/YeuCau
		EcommerceEntities db = new EcommerceEntities();
		public ActionResult YeuCau()
        {
            return View();
        }
		[HttpPost]
		public ActionResult GuiYeuCau(ThongBao x)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
					x.soTB = string.Format("{0:MMddhhmmss}", DateTime.Now);
					x.TKQT = t.TKTV;
					x.TKTV = "admin";
					x.ngayTB= DateTime.Now;
					x.trangThai = "DG";
					db.ThongBaos.Add(x);
					db.SaveChanges();
					trans.Commit();
					ViewBag.sc = "Yêu cầu đã gửi thành công";
					return Redirect("DSYeuCau");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Yêu cầu gửi không thành công, vui lòng xem lại nội dung!";
			return View("YeuCau",x);
		}
		public ActionResult DSYeuCau()
		{

			return View();
		}
		public ActionResult DaDoc(string soTB, string url)
		{
			ThongBao x = db.ThongBaos.Find(soTB);
			x.trangThai = "DDc";
			db.SaveChanges();
			if (url == "Home") return RedirectToAction("Home", "Home", new { Area = "" });
			else return RedirectToAction("Dashboard", "Dashboard", new { Area = "PrivateShop" });

		}

	}
}