using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class TrangThaiController : BaoMatController
    {
		// GET: PrivateShop/TrangThai
		private EcommerceEntities db = new EcommerceEntities();
		public ActionResult TrangThai()
		{
			return View();
		}
		public ActionResult CapNhatThongTin()
		{
			TaiKhoanTV x = Session["DangNhap"] as TaiKhoanTV;
			x.matKhau = "";
			return View(x);
		}
		[HttpPost]
		public ActionResult CapNhat(TaiKhoanTV x)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					TaiKhoanTV y = db.TaiKhoanTVs.Find(x.TKTV);
					if(y.hoDem == null)	y.hoDem = x.hoDem;
					if (y.tenTV == null) y.tenTV = x.tenTV;
					if (y.email == null) y.email = x.email;
					if (y.soDT == null) y.soDT = x.soDT;
					if (y.diaChi == null) y.diaChi = x.diaChi;
					if (y.ngaysinh == null) y.ngaysinh = x.ngaysinh;
					db.SaveChanges();
					trans.Commit();
					ViewBag.sc = "Cập nhật thành công";
					return RedirectToAction("TrangThai");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}

			return View(x);
		}
		[HttpPost]
		public ActionResult DoiMK(TaiKhoanTV x, string mk, string xnmk)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					if (x.matKhau != null && mk != null && xnmk != null)
					{
						TaiKhoanTV y = db.TaiKhoanTVs.Find(x.TKTV);
						x.matKhau=CryptoService.ComputeSha1Hash(x.matKhau);
						if (x.matKhau == y.matKhau)
						{
							if (mk == xnmk)
							{
								y.matKhau = CryptoService.ComputeSha1Hash(mk);
								db.SaveChanges();
								trans.Commit();
								ViewBag.scmk = "Cập nhật thành công";
								return RedirectToAction("TrangThai");
							}
							else
							{
								ViewBag.uscmk = "2 mật khẩu mới không giống nhau";
								return Redirect("CapNhatThongTin");
							}
						}
						else
						{
							ViewBag.uscmk = "Mật khẩu cũ không đúng!";
							return Redirect("CapNhatThongTin");
						}
					}
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			return Redirect("CapNhatThongTin");

		}
		[HttpPost]
		public ActionResult DoiAVT(HttpPostedFileBase HinhDaiDien)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					TaiKhoanTV a = Session["DangNhap"] as TaiKhoanTV;
					if (HinhDaiDien != null)
					{
						// Lưu hình vào bài viết
						string viTri = "/Assets/img/";
						string viTriSv = Server.MapPath("~/" + viTri);
						string PMoRong = Path.GetExtension(HinhDaiDien.FileName);
						string tenF = "HDD" + a.TKTV + PMoRong;
						HinhDaiDien.SaveAs(viTriSv + tenF);
						a.hinhDD = viTri + tenF;
					}
					else a.hinhDD = "";
					TaiKhoanTV b = db.TaiKhoanTVs.Find(a.TKTV);
					b.hinhDD = a.hinhDD;
					db.SaveChanges();
					trans.Commit();
					return Redirect("TrangThai");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			return Redirect("TrangThai");

		}
	}
}