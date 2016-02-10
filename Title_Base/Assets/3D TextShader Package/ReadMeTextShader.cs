/*
Create a New empty gameobject. (Do not start with a UI Text Object)
Attach a Text Mesh to the Game Object. This allows it to function as Text in 3D space, instead of UI space.
Type something into it's text parameter to make sure it's rendering properly.



Create a new Material. Name it (OPTIONALLY) FONT_"FONTNAME"_3DMTL 

Attach the Material to the TextMesh gameobject. (This should turn it into a jumble of squares)
On the TextMesh gameobject, add the FONT_"FONTNAME"_3DMTL material as the only element in the Mesh Renderer component's materials array

Change the FONT_"FONTNAME"_3DMTL shader parameter from Standard to "GUI" -> "3D Text Shader - Cull back". 
This should make your font invisible from behind. (but it's still a jumble of squares)

Import a True Type Font (TTF) file from wherever you want. Note: You cannot use the Default Font in Unity.
Inside the imported TTF is a Font Texture. Apply it as the Font Texture parameter for FONT_"FONTNAME"_3DMTL

*/