# Saas Factory

Many experienced developers are familiar with the phrase **“ideas are cheap, implementation costs.”**  The benefit of this universal truth is that we can afford to have many ideas!

The reality, however, is that *talk is cheap*. The real burden lies in execution. This fact of life continues to plague countless projects, often rendering them impractical before they even get off the ground.

From personal experience, I know how hard it is to resist rewriting projects in pursuit of perfection. Reassurances like *“don’t worry, I’ll reuse as much as possible”* are often in vain as we’re usually rewriting things because we weren’t happy with them in the first place *duh*!

Reflecting on past rewrites, it became clear how easy it is to underestimate the time spent on configuration and setup. Logging, authentication, event sourcing, libraries, frameworks—everything needs to be implemented, wired together, and maintained. 

**I started asking myself, what if:**

- 🚀 It became easier to make changes and quicker to get off the ground?

- 💰 We could reduce the time and cost of failure?

- ♻️ Our new applications could reuse common features and components every time we started a new project with minimal configuration?

- 🤝 What if incremental enhancements made while working on one project could immediately benefit multiple others?

- 🎨 What if our solution could also provide a monorepo where families of products could share the same Ux?

### Research

So the first question I had was **How do other people solve this problem?**

To answer that, I began looking at some of my favorite development family of products, the JetBrains IDEs. 

They all share a common base that is extended for different use cases. This reduces the effort required to build new IDEs and also provides a consistent look, feel, and user experience. The result is a reinforced brand, a reduced learning curve, and significant long-term efficiency gains.

The benefits of this approach extend far beyond developer experience.

I did some further research and  I came across an inspiring video:

[How to Build a SaaS Factory – Ship 10× Faster](https://www.youtube.com/watch?v=z5U843Ob8xw&list=PLndan8QpNLRqgt86TMo4KAq20rWHyfiYk)

So I packed up my keyboard and set out on a new coding adventure. 

And so, that's how we got here. 


## Getting Started
#### Prerequsites
- git - of course!
- .NET SDK
- .NET Aspire Cli
  - install with `dotnet tool install aspire`
- NodeJS - v24.11.1
- pnpm (preferred) - v10.18.0
  - Install with `npm i -g pnpm`

The easiest way to use this repo is to build it. Start by installing the required front end node modules (preferably with pnpm): `pnpm i`. 

Then run `pnpm start` this will serve the app using .NET aspire. It will not automatically open the page (as this can get annoying during development), you will see a link to the page in the terminal window. 

If you have issues reach out to me and we'll go through it on a video call or something.

Take a look at the docs site

## How Does It Work

**So, is this a framework?**
- At the moment this project more of a boilerplate, than a framework, but I see it having the potential to evolve into a proper .NET monorepo framework over time, perhaps.

For now, though, I’m focused on building a solid modular foundation for future projects.  The architecture is based on a modular monolith. However, each module can also provide its own front-end code too. 

It's not perfect, but it works and it's continually getting better. 

### Components

#### Web Project

The ASP.NET app serves as the backbone of the project. It's job is primarily to serve the endpoints provided by the modules. 

### Modules 

There are two types of module: 

1. **Domain Modules** - These are where the developer defines the endpoints, services, and ui and themes for their specific SaaS app. They extend the base to provide product specific functionality.
   
2. **Core Feature Modules** - Core feature modules are built into the foundation. These could have been baked into the web project but are separate modules for now. <br><br>**Note:** The core modules can be directly customised **in the consuming domain SaaS project** by overriding them. Updates to the base will have to be merged into the consumer SaaS app for now. This will need to be improved later down the line, but it works for now.<br><br>**Example core feature modules:**
    - Authentication
      - With the ability to customise the login page.
    - Event sourcing (using Marten)
    - Logging
      - With Loki and Graphana for Dashboards
    - User Subscription Management **(currently in development)**

