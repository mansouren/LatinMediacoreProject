using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace LatinMedia.web.Pages.Admin
{
    [UserRoleChecker]
    public class IndexModel : PageModel
    {
       
        public void OnGet()
        {

        }
    }
}