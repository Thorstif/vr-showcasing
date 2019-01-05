# vr-showcasing
Import 3D models during runtime into a virtual reality environment for inspection and interaction made with Unity (game engine).

This was a school project for a bacherlor's in software engineering.

TODO: Translate Norwegian comments to English.

Disclaimer: As this project uses the Oculus Avatar SDK, a lot of the assets are produced by Oculus. I'm not sure of the licensing in regards to that.


There are some limitations to the model import functionality. First of all, only .obj files are supported. .Obj files have a loose definition on requirements of how to generate the files. This means that each application that can generate .obj files does so in their own unique way. The focus has been on supporting Blender's generated .obj files. Due to being made in Unity v5.5, submeshes can not exceed a vertice count of 65 000, whereas the newest version of Unity supports a much bigger vertice count. Textures are also not fully supported.
