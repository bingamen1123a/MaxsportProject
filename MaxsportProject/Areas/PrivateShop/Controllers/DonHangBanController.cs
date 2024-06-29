using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class DonHangBanController : BaoMatController
	{
		static EcommerceEntities db = new EcommerceEntities();
		// GET: PrivateShop/DonHang
		public ActionResult ChoXacNhan()
        {
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == false && m.tktv1 == t.TKTV && m.trangThai.Equals("")).OrderByDescending(m => m.ngayDat).ToList();
			return View();
        }

        public ActionResult DangXuLy()
        {
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV && m.trangThai != "TC" && m.trangThai != "HUY").OrderByDescending(m => m.ngayDat).ToList();
			return View();
        }
        
        public ActionResult DaGiao() // Đã Giao hàng
		{
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV && m.trangThai == "DG").OrderByDescending(m => m.ngayDat).ToList();
			return View();
        }
        public ActionResult ThanhCong()//Thành công
        {
            TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
            ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV && m.trangThai == "TC").OrderByDescending(m => m.ngayDat).ToList();
            return View();
        }
        public ActionResult DonBiHuy()
        {
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV && m.trangThai == "HUY").OrderByDescending(m => m.ngayDat).ToList();
			return View();
        }
		public ActionResult TatCa()
		{
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV).OrderByDescending(m => m.ngayDat).ToList();
			return View();
		}
		public ActionResult TraHang()
		{
			TaiKhoanTV t = Session["DangNhap"] as TaiKhoanTV;
			ViewData["listDonHang"] = new EcommerceEntities().DonHangs.Where(m => m.daKichHoat == true && m.tktv1 == t.TKTV && m.trangThai == "TH").OrderByDescending(m => m.ngayDat).ToList();
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

			return Redirect("DonBiHuy");
		}
		[HttpPost]
		public ActionResult Active(string maDH)
		{
			DonHang x = db.DonHangs.Find(maDH);
			x.daKichHoat = true;
			db.SaveChanges();
			return Redirect("DangXuLy");
		}
		[HttpPost]
		public ActionResult TC(string maDH) // thành công
		{
			DonHang x = db.DonHangs.Find(maDH);
			x.trangThai = "TC";
			db.SaveChanges();
			return Redirect("ThanhCong");
		}
        [HttpPost]
        public ActionResult DG(string maDH) // đã giao hàng
        {
            DonHang x = db.DonHangs.Find(maDH);
            x.trangThai = "DG";
            db.SaveChanges();
            return Redirect("DangXuLy");
        }
        [HttpPost]
		public ActionResult ChL(string maDH) // chờ lấy
		{
			DonHang x = db.DonHangs.Find(maDH);
			x.trangThai = "ChL";
			db.SaveChanges();
			return Redirect("DangXuLy");
		}
		[HttpPost]
		public ActionResult TH(string maDH) // trả hàng
		{
			DonHang x = db.DonHangs.Find(maDH);
			x.trangThai = "TH";
			db.SaveChanges();
			return Redirect("TraHang");
		}

	}
}