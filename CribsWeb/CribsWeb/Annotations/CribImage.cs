using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cribs.Web.Annotations
{
    public class CribImage : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 3;
            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png"};
            var file =  value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
           else if(!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf(".")))) {
                ErrorMessage = "Please upload a file of type: " + string.Join(",", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength) {
                ErrorMessage = "File too large. Maximum allowed size is " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
