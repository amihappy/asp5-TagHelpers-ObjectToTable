using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using Newtonsoft.Json;

namespace ObjectToTableHelper
{
    [HtmlTargetElement("object-to-table", TagStructure = TagStructure.Unspecified)]
    public class ObjectToTableTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            var c = await output.GetChildContentAsync();
            var input = c.GetContent().Replace("&quot;", "\"");
            Regex r = new Regex("({|,)\\D(?<propName>\\w+)\\D:\\D(?<value>[^\"]+)\\D");
            var matches = r.Matches(input);
            var outputBuilder = new StringBuilder();
            foreach (Match match in matches)
            {
                var prop = match.Groups["propName"];
                var value = match.Groups["value"];
                outputBuilder.Append($"<tr><td>{prop.Value}</td><td>{value.Value}</td><tr>");
            }

            output.Content.SetContent(new HtmlString(outputBuilder.ToString()));
        }
    }
}
