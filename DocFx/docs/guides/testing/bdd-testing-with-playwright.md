# 🧩 BDD Testing with Playwright

This document explains how **BDD-style testing** is implemented across all modules and features in the **SaaS Factory** platform.  
It uses a combination of **[TestStack.BDDfy](https://github.com/TestStack/TestStack.BDDfy)** for behavior-driven test structure and **[Microsoft.Playwright](https://playwright.dev/dotnet/docs/intro)** for browser-based end-to-end automation, all running under **NUnit**.

---

## 🎯 Purpose

Every module and feature in the SaaS Factory must include **automated acceptance tests** written in a **behavior-driven development (BDD)** style.  
These tests ensure that the product behaves as expected from a user perspective — interacting through the browser, navigating through UI flows, and verifying business logic at the system level.

Each BDD test scenario should:

- Describe *user behavior* in natural language (Given–When–Then).
- Execute through a real browser using Playwright.
- Run as part of the overall NUnit test suite.
- Follow the SaaS Factory's consistent structure for maintainability and reporting.

---

## ⚙️ Technology Stack

| Component | Purpose |
|------------|----------|
| **BDDfy** | Provides the BDD structure and reporting (Given/When/Then). |
| **Playwright** | Automates the browser and performs real user interactions. |
| **NUnit** | Serves as the test runner and integrates with CI pipelines. |

---

## 🧱 Structure Overview

Each feature test in the SaaS Factory follows the same pattern:

1. **Test Fixture** (`PageTest`)  
   - Inherits from `Microsoft.Playwright.NUnit.PageTest`.  
   - Manages Playwright lifecycle (browser, context, and page).  
   - Defines one or more `[Test]` methods that run BDD stories.

2. **Story Class** (POCO)  
   - A plain C# class that defines `[Given]`, `[When]`, and `[Then]` steps.  
   - Receives the Playwright `IPage` from the test fixture.  
   - Contains no NUnit or Playwright lifecycle logic — purely the test story.

3. **Base Story Type** (`PlaywrightStory`)  
   - Provides a shared `Page` property for easy access to Playwright within steps.  
   - Ensures consistent story initialization across all tests.

---

## 🧩 Implementation

### 1️⃣ Base Story Type

``` csharp
using Microsoft.Playwright;

namespace SaasFactory.Tests
{
    public abstract class PlaywrightStory
    {
        protected IPage Page { get; }

        protected PlaywrightStory(IPage page)
        {
            Page = page;
        }
    }
}
```

All story classes inherit from `PlaywrightStory` so they automatically have access to the active Playwright `Page`.

---

### 2️⃣ Example Test Fixture

``` csharp
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using TestStack.BDDfy;

namespace SaasFactory.Tests.Features.Homepage
{
    [TestFixture]
    public class PlaywrightHomepage_Spec : PageTest
    {
        [Test]
        public void RunStory()
        {
            var story = new PlaywrightHomepageStory(Page);
            story.BDDfy("Playwright homepage story");
        }
    }
}
```

The fixture inherits from `PageTest`, which handles Playwright’s setup and teardown automatically.  
The `RunStory()` test instantiates the story class and passes it to BDDfy for execution and reporting.

---

### 3️⃣ Example Story Class

``` csharp
using Microsoft.Playwright;
using NUnit.Framework;
using TestStack.BDDfy;

namespace SaasFactory.Tests.Features.Homepage
{
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
```

Each method represents a **step** in the user’s interaction:
- **Given** → initial setup or navigation  
- **When** → user action  
- **Then** → expected outcome or assertion

BDDfy uses these annotated methods to generate readable test reports.

---

## 🧩 Why This Structure

This architecture solves the common issue of BDDfy reflection failures when inheriting directly from `PageTest`.  
By separating the **test fixture** (which manages Playwright’s async lifecycle) from the **story class** (which defines BDD steps), the framework achieves:

- ✅ **Stable reflection** for BDDfy  
- ✅ **Automatic Playwright lifecycle** from NUnit  
- ✅ **Clear separation of concerns**  
- ✅ **Readable and maintainable stories**  
- ✅ **Reusability** across all SaaS Factory features

---

## 🧪 Writing New Feature Tests

When adding a new SaaS Factory feature:

1. **Create a new test fixture** under `SaasFactory.Tests.Features.[FeatureName]`.
2. **Create a corresponding story class** (either in the same file or its own).
3. **Inherit from `PlaywrightStory`** to access `Page`.
4. **Use `[Given]`, `[When]`, `[Then]`** attributes for clarity.
5. **Run locally via NUnit** or through the CI pipeline.

---

## 🧠 Example Folder Layout

```
📂 SaasFactory.Tests
 ├── 📁 Features
 │   ├── 📁 Homepage
 │   │   ├── PlaywrightHomepage_Spec.cs
 │   │   └── (additional stories if needed)
 │   ├── 📁 UserSubscriptions
 │   │   ├── UserSubscription_Spec.cs
 │   │   └── ...
 │   └── 📁 Billing
 │       └── Billing_Spec.cs
 └── 📁 Infrastructure
     └── PlaywrightStory.cs
```

Each feature folder contains its own test specifications and story classes.

---

## 🧭 Summary

| Goal | Implementation |
|------|----------------|
| Behavior-driven acceptance tests | BDDfy `[Given]`, `[When]`, `[Then]` |
| Browser automation | Playwright via `PageTest` |
| Test execution | NUnit |
| Story reusability | `PlaywrightStory` base class |
| CI-ready architecture | Modular, isolated features |

---

## ✅ TL;DR

- All SaaS Factory modules and features **must** include BDD-style Playwright tests.  
- **Never** let a BDDfy story class inherit `PageTest` — use `PlaywrightStory` instead.  
- Keep the fixture small and let BDDfy handle the narrative logic.  
- Maintain one story per logical feature behavior for clarity and reporting.

This pattern keeps SaaS Factory’s test suite consistent, expressive, and production-grade —  
validating every user-facing behavior through real browser automation.
