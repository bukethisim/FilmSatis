﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    //hem class hem interfaceden miras alıyorsak ilk classı yazmalıyız.
    public class OnlyYoungAndAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) //çalışıyorken kullanacaksak
        {
           bool isYoung = filterContext.HttpContext.User.Identity.IsYoung();
           bool isAdmin = filterContext.HttpContext.User.IsInRole("Adminstrator");

            if (isYoung || isAdmin)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = new RedirectResult("/");
        }
    }
}