using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind.Model;
using Northwind.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Northwind.Web.Tests
{
    [TestClass]
    public class CategoriesControllerAcceptanceTests
    {
        private string _indexPath = "http://localhost:5129";
        private string _categoriesPath = "http://localhost:5129/Categories";
        
        [TestMethod]
        public async Task Index_ShouldLinks_ToCategories()
        {
            var client = GetClient();
            var response = await client.GetStringAsync(_indexPath);

            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(response)).Result;

            var links = document.Links
                .OfType<IHtmlAnchorElement>()
                .Select(l => l.Href)
                .Where(x => x.Contains("Categories"));

            links.Should().NotBeNull();
            links.Any().Should().BeTrue();
        }

        [TestMethod]
        public async Task Details_EditLink_ShouldBe_Correct()
        {
            var client = GetClient();
            var response = await client.GetStringAsync($"{_categoriesPath}/Details/1");

            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(response)).Result;

            var links = document.Links
                .OfType<IHtmlAnchorElement>()
                .Select(l => l.Href)
                .Where(x => x.Contains("Categories/Edit/1"));

            links.Should().NotBeNull();
            links.Any().Should().BeTrue();
        }

        [TestMethod]
        public async Task Details_ShouldContain_Data()
        {
            var client = GetClient();
            var response = await client.GetStringAsync($"{_categoriesPath}/Details/1");

            var result = GetCategory(response);

            result.Should().NotBeNull();
            result.CategoryName.Should().NotBeEmpty();
            result.Description.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task Categories_Links_ShouldBe_Correct()
        {
            var client = GetClient();
            var response = await client.GetStringAsync(_categoriesPath);

            var detailsExpect = new Regex(@".*\\Details.\d");
            var deleteExpect = new Regex(@".*\\Delete.\d");
            var editRegex = new Regex(@".*\\Edit.\d");

            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(response)).Result;

            var links = document.Links
                .OfType<IHtmlAnchorElement>()
                .Select(l => l.Href)
                .Where(x => x.Contains("Categories"));

            links.Should().NotBeNull();
            links.Count(x => detailsExpect.IsMatch(x))
                .Should().Be(links.Count(x => editRegex.IsMatch(x)));
            links.Count(x => editRegex.IsMatch(x))
                .Should().Be(links.Count(x => deleteExpect.IsMatch(x)));

        }

        [TestMethod]
        public async Task Categories_ShouldContain_Data()
        {
            var client = GetClient();
            var response = await client.GetStringAsync(_categoriesPath);

            var result = GetResultCategories(response).ToList();

            result.Should().NotBeNullOrEmpty();
            result.ForEach((x) =>
            {
                x.CategoryName.Should().NotBeEmpty();
                x.Description.Should().NotBeEmpty();
            });
        }

        [TestMethod]
        public async Task Delete_ShouldContain_Data()
        {
            var client = GetClient();
            var response = await client.GetStringAsync($"{_categoriesPath}/Delete/2");

            var result = GetCategory(response);

            result.Should().NotBeNull();
            result.CategoryName.Should().NotBeEmpty();
            result.Description.Should().NotBeEmpty();
        }
        private static HttpClient GetClient() => new();

        private static CategoryViewModel GetCategory(string htmlSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(htmlSource)).Result;

            var ddElements = document.QuerySelectorAll("dd");
            var category = new CategoryViewModel();

            category.CategoryName = ddElements.ElementAtOrDefault(0)?.InnerHtml.Trim() ?? "";
            category.Description = ddElements.ElementAtOrDefault(1)?.InnerHtml.Trim() ?? "";

            return category;
        }

        private static IEnumerable<Category> GetResultCategories(string htmlSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(htmlSource)).Result;

            foreach (var categoryRow in document.QuerySelectorAll("tr[data-tid|='category-row']"))
            {
                var id = categoryRow.GetAttribute("data-tid")?.Split("-").Last();
                var name = categoryRow.QuerySelector("td[data-tid='category-name']")?.Text().Trim();
                var description = categoryRow.QuerySelector("td[data-tid='category-description']")?.Text().Trim();

                yield return new Category
                {
                    CategoryId = int.Parse(id ?? "-1"),
                    CategoryName = name ?? "",
                    Description = description,
                    Picture = null,
                };
            }
        }
    }
}
