
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class ImageUtility
    {
        public static string GetDefautImageUrl(IWebHostEnvironment env)
        {
            return Path.Combine(env.WebRootPath, "images", "defaultimage.gif");
        }
        public static string GetAbsoluteUrl(IWebHostEnvironment env, string url)
        {
            return Path.Combine(env.WebRootPath, url);
        }
        public static string GetRelativeUrl(string url)
        {
            return Path.Combine("images", url);
        }
    }
}
