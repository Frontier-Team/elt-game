# Useful Info

Some useful links & info dumps for using Unity that isn't apparent without diving into docs or long use:

## [Special Folders](https://docs.unity3d.com/Manual/SpecialFolders.html) 
Unity reserves names of specific folders in the Assets directory for specific use cases, some examples include: 

| Folder Name              | Description                                                                                                                                                                                                 |
|--------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Editor                   | This is where you'd store editor only scripts that you don't want to be included in the final built executable. Any editor tools, asset scripts, or build time scripts should live here.                    |
| Editor Default Resources | Any assets that should be available to editor only scripts on demand should live here.                                                                                                                      |
| Resources                | Any assets that you want to load on-demand from scripts, rather than hard referenced within scenes or game objects heirarchy should live here.                                                              |
| Plugins                  | 3rd party plugins, platform specific plugins, and any assemblies/scripts that will only be built for specific platforms should be included here. This includes things like android/ios native code/objects. |
| Streaming Assets         | Any assets stored here will be preserved in their original format, useful for web where you want to access raw assets instead of bundled assets that were imported by unity & built directly into your app. |


## [Assembly Definitions & References](https://docs.unity3d.com/6000.0/Documentation/Manual/assembly-definitions-intro.html)
 
You should read up on the basics of assemblies in C# if you don't have experience with them. Specifically in Unity they are very powerful for project maintenance, keeping a clean & performant project to work in. The unity docs linked above cover a good breadth of the topic.

The high level overview is that Unity generates its own assemblies (Assembly-CSharp, Assembly-CSharp-editor, etc) out of your project structure, which will bundle & recompile all other scripts & assets in the project everytime a script changes. This can be fine for small projects, but larger projects with lots of code can quickly become a long wait on a lower performance machine (or even a higher performing one!).

Organising your project into relatively simple assemblies can relieve some of the pain involved with this, but needs careful maintenance & overview to make sure things are being loaded & referenced in correct places.

A good practice is to organise game code into its own assembly, editor code into its own assembly, and core/third party code can be split up as this might not be updated as often, so Unity won't compile & reload every time you change a game script. This might look like this in a project:

```
Assets/
├── _Game/
│   ├── Scripts/ <--- Game-Assembly.asmdef applies to all scripts here 
|   |   ├── Game.cs
|   |   ├── UI.cs
|   │   └── Game-Assembly.asmdef 
│   ├── Scenes/
│   ├── Prefabs/
|   └── Blah/  
├── Core/
|   ├── Scripts/
|   |   ├── Framework1/
|   |   |   ├── FrameworkCode.cs
|   |   |   └── Framework1.asmdef <-- Allows other assemblies to ref Framework1 code (or avoid)
|   |   └── Framework2/
|   |       ├── Framework2Code.cs
|   |       └── Framework2.asmdef <-- Allows other assemblies to ref Framework2 code (or avoid)
├── Editor/
    └── Scripts/ <--- Unity special folders catches this normally
        ├── EditorTool.cs
        └── Editor-Assembly.asmdef <-- Manually configure assemblies referenced by editor 
├── Plugins/
    ├── Android
        └── Android-Assembly.asmdef <-- Allows Android only code separated from rest of codebase
    └── WebGL
        └── WebGL-Assembly.asmdef <-- Allows WebGL only code separated from rest of codebase
├── Resources/
├── StreamingAssets/
└── ThirdParty/
    └── Analytics/
        ├── AnalyticsManager.cs
        └── Analytics.asmdef <-- Expose analytics manager to the rest of the code manually
``````
In simplest terms, we've split our assembly dependencies & references manually rather than allowing unity to decide where things are included. This adds complexity in the short term, but long term helps us manage things, so should be used early. 

This allows us to split away any core technology we might want to share between projects into packages with relative ease, because we will already understand which code depends on each other, rather than dissassembling the spaghetti of a game project later down the line.