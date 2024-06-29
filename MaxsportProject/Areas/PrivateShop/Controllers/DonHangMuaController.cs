using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class DonHangMuaController : BaoMatController
    {
		// GET: PrivateShop/DonHangMua
		static EcommerceEntities db = new EcommerceEntities();
		public ActionResult DSDonHang()
		{
			return View();
		}
		[HttpPost]
		public ActionResult XemChiTiet(string maDH)
		{
			DonHang x = db.DonHangs.Find(maDH);
			List<CtDonHang> list = DataIn.GetCtDonHangs(maDH);
			ViewData["DH"] = x;
			ViewData["CTDH"] = list;
			ViewData["SH"] = DataIn.GetTaiKhoanTV(x.tktv1);
			ViewData["KH"] = DataIn.GetGiaoHang(x.tktv2);
			return View();
		}
		[HttpPost]
		public ActionResult HUY(string maDH)
		{
			DonHang x = db.DonHangs.Find(maDH);
			x.trangThai = "HUY";
			db.SaveChanges();
			return View("DSDonHang");
		}
    }
}