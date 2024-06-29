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
    public class NganhHangController : QTController
	{
        // GET: PrivateShop/NganhHang
		static EcommerceEntities db = new EcommerceEntities();
		// Kiểm tra xem có phải người dùng thực hiện lệnh Update cho 1 sản phẩm hay loại sản phẩm nào đó
		private static bool isUpdate = false;
		public ActionResult NganhHang()
        {
            return View();
        }
		/// <summary>
		/// Phương thức thêm ngành hàng mới
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult NganhHang(NganhHang x, HttpPostedFileBase HinhDaiDien)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					if (!isUpdate)
					{
						if (x.ghiChu == null) x.ghiChu = "";
						if (HinhDaiDien != null)
						{
							// Lưu hình vào bài viết
							string viTri = "/Assets/img/";
							string viTriSv = Server.MapPath("~/" + viTri);
							string PMoRong = Path.GetExtension(HinhDaiDien.FileName);
							string tenF = "HDDnganhHang" + x.maNganh + PMoRong;
							HinhDaiDien.SaveAs(viTriSv + tenF);
							x.hinhDD = viTri + tenF;
						}
						else x.hinhDD = "";
						db.NganhHangs.Add(x);
					}
					else
					{
						NganhHang y = db.NganhHangs.Find(x.maNganh);
						y.tenNganh = x.tenNganh;
						if (x.ghiChu != null) y.ghiChu = x.ghiChu;
						if (HinhDaiDien != null)
						{
							string filePath = Server.MapPath("~" + x.hinhDD);
							if (System.IO.File.Exists(filePath))
							{
								System.IO.File.Delete(filePath);
							}
							// Lưu hình vào bài viết
							string viTri = "/Assets/img/";
							string viTriSv = Server.MapPath("~/" + viTri);
							string PMoRong = Path.GetExtension(HinhDaiDien.FileName);
							string tenF = "HDDnganhHang" + y.maNganh + PMoRong;
							HinhDaiDien.SaveAs(viTriSv + tenF);
							y.hinhDD = viTri + tenF;
							isUpdate = false;
						}
						else isUpdate = false;
						isUpdate = false;
					}

					// lưu vào cơ sở dữ liệu
					db.SaveChanges();
					trans.Commit();
					ModelState.Clear();
					ViewBag.sc = "Cập nhật thành công";
					return View();

				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công";	
			return View(x);
		}
		[HttpPost]
		public ActionResult DeleteNH(string maNganh)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					int ma = int.Parse(maNganh);
					//---Tìm đối tượng loại sản phẩm trong dữ liệu
					NganhHang x = db.NganhHangs.Find(ma);
					string filePath = Server.MapPath("~"+x.hinhDD);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					//--- Xóa Loại sản phẩm trong danh sách 
					db.NganhHangs.Remove(x);
					//---Cập nhật list trên View DBase
					db.SaveChanges();
					trans.Commit();
					//-- Đọc danh sách dữ liệu từ DBase
					return View("NganhHang");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công, do có loại hàng đang thuộc ngành hàng này.";
			return View("NganhHang");

		}
		[HttpPost]
		public ActionResult UpdateNH(string maNganh)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					int ma = int.Parse(maNganh);
					//---Tìm đối tượng loại sản phẩm trong dữ liệu
					NganhHang x = db.NganhHangs.Find(ma);
					isUpdate = true;
					//---
					return View("NganhHang", x);
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công";
			return View("NganhHang");

		}
	}
}