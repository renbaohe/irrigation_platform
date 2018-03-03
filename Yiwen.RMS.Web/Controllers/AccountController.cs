using System.Web.Mvc;
using System.Web.Security;
using Yiwen.RMS.Bll;
using Yiwen.RMS.Data;
using Yiwen.RMS.Web.Models;

namespace Yiwen.RMS.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        UserService us = new UserService();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
                if (CheckUser(model))
                {
                    users usinfo = us.DoLogin(model.UserName, model.Password).Data;
                    FormsAuthentication.SetAuthCookie(usinfo.id.ToString(), false);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                    return View(model);
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ModelState.AddModelError("", "无效的模型");
            return View(model);
        }

        private bool CheckUser(LoginModel model)
        {
            return us.DoLogin(model.UserName, model.Password);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #region 帮助程序
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }




        #endregion
    }
}
