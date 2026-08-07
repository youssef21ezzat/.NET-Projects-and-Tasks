using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StudentPortalWeb.TagHelpers
{
    [HtmlTargetElement("year-chip", TagStructure = TagStructure.WithoutEndTag)]
    public class YearChipTagHelper : TagHelper
    {
        public int For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("title", "rendered by youssef");

            if (For == 3)
            {
                output.Attributes.SetAttribute("class", "badge bg-warning text-dark");
                output.Content.SetContent("Year 3");
            }
            else
            {
                output.Attributes.SetAttribute("class", "badge bg-light text-dark");
                output.Content.SetContent($"Year {For}");
            }
        }
    }
}
