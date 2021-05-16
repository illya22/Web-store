using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using Store.Web.Models;

namespace Store.Web.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString Page_Links(this HtmlHelper html, PaginInfo paginInfo,
            Func<int,string> page_Url)
        {
            StringBuilder rez = new StringBuilder();
            for(int i = 1; i<= paginInfo.Total_Pages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", page_Url(i));
                tag.InnerHtml = i.ToString();
                if(i == paginInfo.Current_Page)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                rez.Append(tag.ToString());
            }
            return MvcHtmlString.Create(rez.ToString());
        }
    }
}