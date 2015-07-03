using Cribs.Web.Models;
using Cribs.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cribs.Web.Helpers
{
    public static class CribCustomHtmlHelper
    {
        public static string CardView(this HtmlHelper helper, int count, CardViewSetting setting)
        {
            var NumberOfItems = count;//model.Count();
            var NumberOfPages = NumberOfItems / setting.PageSize;
            if (NumberOfItems % setting.PageSize > 0)
            {/*IEnumerable<ThumbnailsViewModel> list*/
                NumberOfPages += 1;
            }
            
            TagBuilder builder = new TagBuilder("div");
            builder.AddCssClass("row");
            builder.InnerHtml = AddPagination(NumberOfPages);
            return builder.ToString(TagRenderMode.EndTag);

        }

        private static string AddPagination(int numberOfPages)
        {
            TagBuilder builder = new TagBuilder("ul");
            builder.AddCssClass("pagination");
            builder.InnerHtml = AddPages(numberOfPages);
            return builder.ToString(TagRenderMode.EndTag);
        }

        private static string AddPages(int numberOfPages)
        {
            StringBuilder pages = new StringBuilder();
            TagBuilder builder;
            for(int i = 1; i <= numberOfPages; i++)
            {
                builder = new TagBuilder("li");
                builder.Attributes.Add("data-page", string.Join("",i));
                var innerHtml = "<a href='#'>" + i + "</a>";
                builder.InnerHtml = innerHtml;
                pages.Append(builder.ToString(TagRenderMode.EndTag));
            }
            return pages.ToString();           
        }

        private static TagBuilder GetCardLink(string viewAction)
        {
            TagBuilder atag = new TagBuilder("a");
            atag.Attributes.Add("href", viewAction);
            return atag;
        }

        private static TagBuilder GetCardContainer()
        {
            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("col-md-3");
            container.AddCssClass("thumbnail");
            return container;
        }
    }
}
