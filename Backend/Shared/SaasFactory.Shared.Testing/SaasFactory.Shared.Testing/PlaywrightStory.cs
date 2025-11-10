using Microsoft.Playwright;

namespace SaasFactory.Shared.Testing;

/// <summary>
/// Represents an abstract base class for Playwright-based test stories.
/// </summary>
/// <remarks>
/// This class provides a common foundation for browser-driven test scenarios 
/// using Microsoft Playwright. It encapsulates a shared <see cref="IPage"/> instance 
/// that derived classes can use to perform interactions and assertions against the UI.
/// </remarks>
public abstract class PlaywrightStory
{
    /// <summary>
    /// Gets the Playwright <see cref="IPage"/> instance used to drive browser interactions.
    /// </summary>
    protected readonly IPage Page;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightStory"/> class.
    /// </summary>
    /// <param name="page">
    /// The Playwright <see cref="IPage"/> instance representing the current browser page 
    /// used for executing automated test steps.
    /// </param>
    protected PlaywrightStory(IPage page)
    {
        Page = page;
    }
}
