# SaaS Factory

A starter project for building SaaS applications with: 
- A component library built with Tailwind, and Storybook integrations.
- A Modular Dotnet backend with 
    - Event Sourcing
    - Aspire
    - Keycloak Authentication Set up and ready to go

## Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/TheDevelolper/Saas-Factory.git
cd saas-factory
```

### 2. Install dependencies
```bash
pnpm install
```

### 4. Run Storybook
```bash
pnpm storybook
```

### Building Your App

Create your own application and set SaaS Factory as an upstream to pull updates and stay in sync with the starter project.

#### 1. Create a new repository

Go to GitHub and create a new repository for your project, e.g., `my-saas-app`.

#### 2. Clone your new repository

```bash
git clone https://github.com/your-username/my-saas-app.git
cd my-saas-app
```

3. Add SaaS Factory as an upstream remote
```bash
git remote add upstream https://github.com/TheDevelolper/Saas-Factory.git
```

4. Fetch upstream branches
```bash
git fetch upstream
```

5. Create a branch for syncing upstream (optional)
```bash
git checkout -b upstream-sync upstream/main
```

6. Merge or rebase upstream changes into your main branch

```bash
git merge upstream-sync
```


#### Creating API Modules
You can create api modules in the .NET backend see the example banking module for more details

#### Building Frontend Applications
Common components can be put into the Saas Factory base project and domain specific ones should be placed in your own project.

As a general rule of thumb test common components in your own clone before you contribute them into this repo.