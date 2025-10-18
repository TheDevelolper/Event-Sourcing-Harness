# Saas Factory

## Getting Started

The easiest way to use this repo is to build it along side your current project in a separate repository.

Start your new github repo and simply follow these instructions link this Saas Factory into your project's repo:

### 🧩 Linking the Base Repo (`Saas-Factory`) as a Branch
The easiest way to use this repo is to build it along side your current project in a separate repository.

Start your new github repo and simply follow these instructions link this Saas Factory into your project's repo:

To work on both your **project repo** and the **base repo (`Saas-Factory`)** within the same repository, you can add the base repo as a remote and create a local branch that tracks it.  

This allows you to switch between the project and base code using Git branches, and push updates directly to each repo.

Follow these steps:

- Navigate to your project repository
- Link project with the Saas Factory: 

``` powershell
git remote add saas-factory https://github.com/TheDevelolper/Saas-Factory.git   # Add the base repo as a remote
git fetch saas-factory                                                          # Fetch all branches from the base repo
git switch --create saas-factory saas-factory/main                              # Create a new branch tracking the base repo's main branch
git branch --set-upstream-to=saas-factory/main saas-factory                     # Allows you to push changes on this branch back into the saas-factory
git switch main                                                                 # Switch back to your project branch
```
To merge the saas-factory into your local branch you simply merge it as you would do with any regular local branch.

``` powershell
git merge saas-factory                                                  # Merge the base repo into your project
```

Pushing to the saas factory repo: 

1. Switch to the Saas Factory branch: 

``` powershell
git switch saas-factory
```
2. Do some work

3. Commit as you normally would

4. Push
``` pwsh 
git push saas-factory HEAD:main
```