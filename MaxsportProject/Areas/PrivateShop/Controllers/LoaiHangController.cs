using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class LoaiHangController : BaoMatController
	{
		EcommerceEntities db = new EcommerceEntities();
		// Kiểm tra xem có phải người dùng thực hiện lệnh Update cho 1 sản phẩm hay loại sản phẩm nào đó
		private static bool isUpdate = false;
		// GET: PrivateShop/LoaiHang
		public ActionResult LoaiHang()
        {
			List<LoaiSP> list = DataIn.GetLoaiSPs();
			ViewData["list"] = list;
			return View();
		}
		[HttpPost]
		public ActionResult LocNganh(int maNganh)
		{
			List<LoaiSP> list = DataIn.GetLSPTheoNganh(maNganh);
			ViewData["list"] = list;
			return View("LoaiHang");
		}
		[HttpPost]
		public ActionResult LoaiHang(LoaiSP x)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					if (!isUpdate)
					{
						if (x.ghiChu == null) x.ghiChu = "";
						db.LoaiSPs.Add(x);
					}

					else
					{
						LoaiSP y = db.LoaiSPs.Find(x.maLoai);
						y.tenLoai = x.tenLoai;
						y.maNganh = x.maNganh;
						if (x.ghiChu != null) y.ghiChu = x.ghiChu;
						isUpdate = false;
					}

					// lưu vào cơ sở dữ liệu
					db.SaveChanges();
					trans.Commit();
					ModelState.Clear();
					ViewBag.sc = "Cập nhật thành công";
					ViewData["list"] = DataIn.GetLoaiSPs();
					return View();
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công";
			ViewData["list"] = DataIn.GetLoaiSPs();
			return View(x);
		}

	}
}