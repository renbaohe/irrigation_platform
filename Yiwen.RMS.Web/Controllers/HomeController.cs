using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yiwen.RMS.Data;
using Yiwen.RMS.Bll;

namespace Yiwen.RMS.Web.Controllers
{
     [Authorize]
    public class HomeController : Controller
    {

        UserService us = new UserService();

        public ActionResult Index()
        {
            users user = us.FindByKey(int.Parse(this.User.Identity.Name)).Data;
            return View(user);
            
        }

    }
    
}
