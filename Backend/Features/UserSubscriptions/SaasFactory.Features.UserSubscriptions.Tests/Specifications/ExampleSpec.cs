using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SaasFactory.Shared.Testing;
using TestStack.BDDfy;

namespace SaasFactory.Features.UserSubscriptions.Tests.Specifications
{
    [TestFixture]
    public class ExampleSpec : PageTest
    {
        [Test]
        public void RunStory()
        {
            var story = new PlaywrightHomepageStory(Page); // pass the Playwright IPage
            story.BDDfy("Playwright homepage story");
        }
    }
    
    file class PlaywrightHomepageStory(IPage page) : PlaywrightStory(page)
    {
        [Given("I am on the Playwright homepage")]
        public async Task GivenIAmOnThePlaywrightHomepage()
        {
            await Page.GotoAsync("https://playwright.dev");
        }

        [When("I check the title")]
        public async Task WhenICheckTheTitle()
        {
            var title = await Page.TitleAsync();
            Assert.That(title, Does.Contain("Playwright"));
        }

        [Then("I can find the Get Started link")]
        public async Task ThenICanFindTheGetStartedLink()
        {
            var link = Page.Locator("text=Get started").First;
            Assert.That(link, Is.Not.Null, "Expected to find the 'Get started' link");
        }
    }
}