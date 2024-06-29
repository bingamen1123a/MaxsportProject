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
    public class SanPhamController : BaoMatController
	{
		// GET: PrivateShop/SanPham
		EcommerceEntities db = new EcommerceEntities();
		private static bool isUpdate = false;
		public ActionResult DSSanPham()
		{
			List<NganhHang> listNganh = DataIn.GetNganhHangs();
			ViewData["listNganh"] = listNganh;
			List<LoaiSP> listLoai = DataIn.GetLoaiSPs();
			ViewData["listLoai"] = listLoai;
			List<SanPham> listSP = DataIn.GetSanPhams();
			ViewData["listSP"] = listSP;
			return View();
		}
		[HttpPost]
		public ActionResult LocNganh(int maNganh,String view)
		{
			List<LoaiSP> list = DataIn.GetLSPTheoNganh(maNganh);
			ViewData["listNganh"] = DataIn.GetNganhHangs();
			ViewData["listLoai"] = list;
			List<SanPham> listSP = new List<SanPham>();
			foreach (var i in list)
			{
				foreach (var r in DataIn.GetSPTheoLoai(i.maLoai))
				{
					listSP.Add(r);
				}
			}
			ViewData["listSP"] = listSP;
			return View(view);
		}
		[HttpPost]
		public ActionResult LocLoai(int maLoai, String view)
		{
			List<SanPham> listSP = DataIn.GetSPTheoLoai(maLoai);
			ViewData["listNganh"] = DataIn.GetNganhHangs();
			ViewData["listLoai"] = DataIn.GetLoaiSPs();
			ViewData["listSP"] = listSP;
			return View(view);
		}
		public ActionResult ThemSPMoi()
        {
			List<NganhHang> listNganh = DataIn.GetNganhHangs();
			ViewData["listNganh"] = listNganh;
			List<LoaiSP> listLoai = DataIn.GetLoaiSPs();
			ViewData["listLoai"] = listLoai;
			SanPham sp = new SanPham();
			sp.ngayDang = DateTime.Now;
			sp.taiKhoan = (Session["DangNhap"] as TaiKhoanTV).TKTV;
			isUpdate = false;
			return View(sp);
        }
		[HttpPost]
		public ActionResult ThemSPMoi(SanPham x, HttpPostedFileBase HinhDaiDien)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					if (!isUpdate) // dành cho thêm mới
					{
						// B1: Xử lý thông tin nhận về từ View
						x.maSP = string.Format("{0:ddMMyyhhmmssfff}", DateTime.Now);
						x.ngayDang = DateTime.Now;
						x.taiKhoan = (Session["DangNhap"] as TaiKhoanTV).TKTV;
						x.daDuyet  = true;
						if (x.giaBan == null) x.giaBan = 0;
						if (x.giamGia == null) x.giamGia = 0;
						if (x.ndTomTat == null) x.ndTomTat = "";

						if (HinhDaiDien != null)
						{
							// Lưu hình vào bài viết
							string viTri = "/Assets/img/";
							string viTriSv = Server.MapPath("~/" + viTri);
							string PMoRong = Path.GetExtension(HinhDaiDien.FileName);
							string tenF = "HDD" + x.maSP + PMoRong;
							HinhDaiDien.SaveAs(viTriSv + tenF);
							x.hinhDD = viTri + tenF;
						}
						else x.hinhDD = "";
						db.SanPhams.Add(x);
					}
					else
					{
						SanPham a = db.SanPhams.Find(x.maSP);
						if (x.ngayDang != null) a.ngayDang = DateTime.Now;
						if (x.tenSP != null) a.tenSP = x.tenSP;
						if (x.giaBan != null) a.giaBan = x.giaBan;
						if (x.giamGia != null) a.giamGia = x.giamGia;
						if (x.ndTomTat != null) a.ndTomTat = x.ndTomTat;
						a.maLoai = x.maLoai;
					    a.noiDung = x.noiDung;
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
							string tenF = "HDD" + a.maSP + PMoRong;
							HinhDaiDien.SaveAs(viTriSv + tenF);
							a.hinhDD = viTri + tenF;
							isUpdate = false;
						}
						else isUpdate = false;
					}
					db.SaveChanges();
					trans.Commit();
					ModelState.Clear();
					ViewBag.sc = "Cập nhật thành công";
					ViewData["listNganh"] = DataIn.GetNganhHangs();
					ViewData["listLoai"] = DataIn.GetLoaiSPs();
					return View("ThemSPMoi");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string s = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công";
			return View(x);
		}
		[HttpPost]
		public ActionResult DeleteSP(string masp)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					//---Dùng lệnh để xóa bài viết dựa vào mã bài viết
					SanPham x = db.SanPhams.Find(masp);
					string filePath = Server.MapPath("~" + x.hinhDD);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					db.SanPhams.Remove(x);
					//---Cập nhật Database
					db.SaveChanges();
					trans.Commit();
					//---Hiển thị lại danh sách với các danh sách sau cập nhật
					//if (x.daDuyet == true) { return View("DSSanPham"); }
					//else return View("SPNgungKD");
					return Redirect("DSSanPham");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công!";
			return Redirect("DSSanPham");
			
		}
		[HttpPost]
		public ActionResult ActiveSP(string masp)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					//---Dùng lệnh để cấm bài viết dựa vào mã bài viết
					SanPham x = db.SanPhams.Find(masp);
					x.daDuyet = !x.daDuyet;
					//---Cập nhật Database
					db.SaveChanges();
					trans.Commit();
					//---Hiển thị lại danh sách với các danh sách sau cập nhật
					//if (x.daDuyet == true) { return View("SPNgungKD"); }
					//else
					return Redirect("DSSanPham");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công!";
			return Redirect("DSSanPham");
		}
		public ActionResult UpdateSP(string masp)
		{
			//---Tìm đối tượng bài viết trong cs dữ liệu
			SanPham x = db.SanPhams.Find(masp);
			isUpdate = true;
			List<NganhHang> listNganh = DataIn.GetNganhHangs();
			ViewData["listNganh"] = listNganh;
			List<LoaiSP> listLoai = DataIn.GetLoaiSPs();
			ViewData["listLoai"] = listLoai;
			return View("ThemSPMoi", x);
		}
	}
}