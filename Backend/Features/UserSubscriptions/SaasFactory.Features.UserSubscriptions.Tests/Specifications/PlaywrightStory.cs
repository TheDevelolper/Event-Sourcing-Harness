using Microsoft.Playwright;

namespace Demo.Tests;

internal abstract class PlaywrightStory
{
    protected readonly IPage Page;

    protected PlaywrightStory(IPage page)
    {
        Page = page;
    }
}