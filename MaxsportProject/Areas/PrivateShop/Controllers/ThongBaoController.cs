using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class ThongBaoController : QTController
    {
		// GET: PrivateShop/ThongBao
		EcommerceEntities db = new EcommerceEntities();
		public ActionResult ThongBao()
        {

            return View();
        }
		[HttpPost]
		public ActionResult GuiThongBao(ThongBao x)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					x.soTB = string.Format("{0:MMddhhmmss}", DateTime.Now);
					x.TKQT = "admin";
					x.ngayTB = DateTime.Now;
					x.trangThai = "";
					db.ThongBaos.Add(x);
					db.SaveChanges();
					trans.Commit();
					ModelState.Clear();
					ViewBag.sc = "Thông báo đã gửi thành công";
					return View("ThongBao");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Thông báo gửi không thành công, vui lòng xem lại nội dung hoặc chủ đề!";
			return View("ThongBao",x);
		}
	}
}