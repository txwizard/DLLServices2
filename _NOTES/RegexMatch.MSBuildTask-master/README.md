# RegexMatch.MSBuildTask

[![NuGet Package](https://img.shields.io/nuget/v/RegexMatch.MSBuildTask.svg)](https://www.nuget.org/packages/RegexMatch.MSBuildTask/)


## Summary

This is a NuGet package for Visual Studio MSBuild task.

This package provide **"RegexMatch"** task for MSBuild script.

## How to use?

```xml
<PropertyGroup>
  <Greeting>Hello Mr.Smith.</Greeting>
</PropertyGroup>
...
<Target ...>
  <!-- After install this NuGet package, -->
  <!-- you can use "RegexMatch" task in your MSBuild script, -->
  <!-- and it's execute regular expression pattern match. -->
  <RegexMatch Input="$(Greeting)" Pattern="^Hello (.+)\.$">
    <!-- Extract result to MSBuild property "Foo". -->
    <Output TaskParameter="Match2" PropertyName="Foo" />
  </RegexMatch>
  
  <!-- Show result. -->
  <Message Text="Nice to meet you, $(Foo)."/>
</target>
```

## Install

    PM> Install-Package RegexMatch.MSBuildTask


## license

[Mozilla Public License Version 2.0](lICENSE)