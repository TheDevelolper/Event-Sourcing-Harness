# Restaurant Menu Example Module

This module serves as an example of how to combine a feature slicing architecture in this project boilerplate. 

## Creating Your Own Module

This easiest way to get started is to copy this module and use it to create a new one by renaming the project
and refactoring the root namespaces. However if you want to create a project from scratch you will need to do the 
following:

Edit the project XML and ensure it contains this line: 

```xml
<ItemGroup>
  <FrameworkReference Include="Microsoft.AspNetCore.App" />
</ItemGroup>
```
It adds a framework reference which allows you to define minimal api endpoints into your module. 
