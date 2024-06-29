using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class QTVController : QTController
    {
		// GET: PrivateShop/QTV
		private static bool isUpdate = false;
		EcommerceEntities db = new EcommerceEntities();
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
		public ActionResult DSSP()//Danh sách sản phẩm
		{
			return View();
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
					return Redirect("DSSP");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công!";
			return Redirect("DSSP");

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
					return Redirect("DSSP");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công!";
			return Redirect("DSSP");
		}
		public ActionResult DSTK()
		{
			return View();
		}
		public ActionResult LockTK(string tktv) // khoá tài khoản
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					TaiKhoanTV a = db.TaiKhoanTVs.Find(tktv);
					a.trangThai = false;
					foreach(var i in DataIn.GetSanPhams())
					{
						if (i.taiKhoan == a.TKTV)
						{
							SanPham sp = db.SanPhams.Find(i.maSP);
							sp.daDuyet = false;
							db.SaveChanges();
						}

					}
					foreach (var i in DataIn.GetDonHangsban(a.TKTV))
					{
						if (i.trangThai != "HUY" || i.trangThai != "TC" || i.trangThai != "TH")
						{
							DonHang x = db.DonHangs.Find(i.soDH);
							x.trangThai = "HUY";
							db.SaveChanges();
						}
					}
					foreach (var i in DataIn.GetDonHangsmua(a.TKTV))
					{
						if (i.trangThai != "HUY" || i.trangThai != "TC" || i.trangThai != "TH")
						{
							DonHang x = db.DonHangs.Find(i.soDH);
							x.trangThai = "HUY";
							db.SaveChanges();
						}
					}
					db.SaveChanges();
					trans.Commit();
					ViewBag.sc = "Cập nhật thành công";
					return Redirect("DSTK");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công";
			return Redirect("DSTK");
		}
		public ActionResult ActiveTK(string tktv) //Kích hoạt tài khoản
		{
			TaiKhoanTV a = db.TaiKhoanTVs.Find(tktv);
			a.trangThai = true;
			db.SaveChanges();
			return View("DSTK");
		}
		public ActionResult XoaDH(string maDH) // xoá đơn hàng
		{
			foreach (CtDonHang i in (new EcommerceEntities().CtDonHangs.Where(m => m.soDH == maDH).ToList()))
			{
				CtDonHang a = db.CtDonHangs.Find(maDH, i.maSP);
				db.CtDonHangs.Remove(a);
			}
			DonHang x = db.DonHangs.Find(maDH);
			db.DonHangs.Remove(x);
			db.SaveChanges();
			return Redirect("DSDonHang");
		}
		public ActionResult DSYC()//Danh sách Yêu cầu
		{
			return View();
		}
		public ActionResult CTDH(string maDH)//Chi tiết đơn hàng
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
		public ActionResult Duyet(string soTB)
		{
			ThongBao a = db.ThongBaos.Find(soTB);
			a.trangThai = "DD";
			db.SaveChanges();
			return View("DSYC");
		}
		[HttpPost]
		public ActionResult TuChoi(string soTB)
		{
			ThongBao a = db.ThongBaos.Find(soTB);
			a.trangThai = "TC";
			db.SaveChanges();
			return View("DSYC");
		}
		[HttpPost]
		public ActionResult XoaYC(string soTB)
		{
			ThongBao a = db.ThongBaos.Find(soTB);
			db.ThongBaos.Remove(a);
			db.SaveChanges();
			return View("DSYC");
		}
		public ActionResult DoiMK()
		{
			TaiKhoanTV x = Session["DangNhap"] as TaiKhoanTV;
			x.matKhau = "";
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
						x.matKhau = CryptoService.ComputeSha1Hash(x.matKhau);
						if (x.matKhau == y.matKhau)
						{
							if (mk == xnmk)
							{
								y.matKhau = CryptoService.ComputeSha1Hash(mk);
								db.SaveChanges();
								trans.Commit();
								ViewBag.scmk = "Cập nhật thành công";
								return RedirectToAction("DSTK");
							}
							else
							{
								ViewBag.uscmk = "2 mật khẩu mới không giống nhau";
								return View("DoiMK");
							}
						}
						else
						{
							ViewBag.uscmk = "Mật khẩu cũ không đúng!";
							return View("DoiMK");
						}
					}
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			return Redirect("DoiMK");

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
		[HttpPost]
		public ActionResult DeleteLoaiSP(string maLoai)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					int ma = int.Parse(maLoai);
					//---Tìm đối tượng loại sản phẩm trong dữ liệu
					LoaiSP x = db.LoaiSPs.Find(ma);
					//--- Xóa Loại sản phẩm trong danh sách 
					db.LoaiSPs.Remove(x);
					//---Cập nhật list trên View DBase
					db.SaveChanges();
					trans.Commit();
					//-- Đọc danh sách dữ liệu từ DBase
					return View("LoaiHang");
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công, do có sản phẩm đang thuộc loại hàng này.";
			return View("LoaiHang");


		}
		[HttpPost]
		public ActionResult UpdateLoaiSP(string mlupdate)
		{
			using (DbContextTransaction trans = db.Database.BeginTransaction())
			{
				try
				{
					int ma = int.Parse(mlupdate);
					//---Tìm đối tượng loại sản phẩm trong dữ liệu
					LoaiSP x = db.LoaiSPs.Find(ma);
					isUpdate = true;
					ViewData["list"] = DataIn.GetLoaiSPs();
					return View("LoaiHang", x);
				}
				catch (Exception ex)
				{
					trans.Rollback();
					string e = ex.Message;
				}
			}
			ViewBag.usc = "Cập nhật không thành công.";
			ViewData["list"] = DataIn.GetLoaiSPs();
			return View("LoaiHang");
		}
		// GET: PrivateShop/LoaiHang
		public ActionResult LoaiHang()
		{
			List<LoaiSP> list = DataIn.GetLoaiSPs();
			ViewData["list"] = list;
			return View();
		}
        public ActionResult QuyenQT(string tktv) //Kích hoạt tài khoản quản trị
        {
            TaiKhoanTV a = db.TaiKhoanTVs.Find(tktv);
            a.ghiChu = "QTV";
            db.SaveChanges();
            return View("DSTK");
        }
        public ActionResult HuyQT(string tktv) //Huỷ tài khoản quản trị
        {
            TaiKhoanTV a = db.TaiKhoanTVs.Find(tktv);
            a.ghiChu = "";
            db.SaveChanges();
            return View("DSTK");
        }


    }
}