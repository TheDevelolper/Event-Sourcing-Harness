# Introduction

## Situation
As a hobbyist developer with broad interests, I often start many projects to test out new technologies and ideas.  

Managing those projects is a laborious task due to things like dependency management. Also, when I learn a new technology, it was impossible to go back and update all my existing repositories.  

So many of them decay with time. I think they call that "entropy growth," described in the second law of thermodynamics.  

### Design Goals

- Easy to extend
- Easy to maintain
- Serve as a platform to make development faster!
- Allow me to make improvements to multiple projects simultaneously.
- Be fast by being lean!


## Task
I needed a way to combat these issues. My solution has been to create a base project where all of the repetitive tasks like logging, documentation, authentication, and general project setup can be centrally managed.  

Not only will this help me maintain my existing projects easily, but I also hope to build future projects much faster.  

## Actions
I initially started with what I knew best, using an Nx Monorepo to kick off the project. However, I found that this added additional dependency overhead and impacted build times too. So I moved away from this in favor of pnpm workspaces, which are far more lightweight.  

## Features
Thus far I've implemented:  
- .NET Aspire to manage development environment setup  
- An extensible modular monolith API where new applications can be registered  
- A DocFx documentation site  
- An Atomic component library framework with some examples  
- Storybook to showcase the component library  
- Reusable authentication provided by Keycloak, with customization using KeyCloakify  

## Future Work

So much to do, so little time! I've begun layout out the foundations for my future projects and I think I've achieved a lot in a short amount of time. That being said I have some future plans too!

### Feature Toggling

I had already implemented this using FeatureHub, it worked well, however I felt that it wasn't simple enough. I now intend to implement feature toggling using Microsoft's Feature newer Toggling mechanism.

### Centralized Logging

I intend to implement elastic search based centralised logging which will be exposed via aspire.

### User Management Theming

The authentication theming works for the login page. I also need to implement it for other pages like user management. It would be great if I could link this with my tailwind theming mechanism. 

### Base Component Library

Work on creating the base application's component library with the most common components has already been started. I hope to grow these components in time based on real world usage. 

- Improve already started theme mechanism
    - Pay attention to accessibility using storybook's accessibility plugins
    - Better component testing

### Documentation Site Theming

The documentation site theme should be fed from the centralised theme mechanism. Though the modern theme will suffice for now. I've got experience with this already. I just need time to implement it.