using Ecommerce_KTPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_KTPM.Areas.PrivateShop.Controllers
{
    public class DashBoardController : BaoMatController
	{
        // GET: PrivateShop/DashBoard
        public ActionResult DashBoard()
        {
            return View(); 
        }
    }
}