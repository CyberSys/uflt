TODO:

-Records:-

LOD
External Ref


-Some notes to myself:-

Level field could be used to set level in model?

Use the face lit field to set the shader that the material uses..
e.g flat, lit, gourad etc.

Need a way to create a material based on:

-Material Palette
-Texture
-Face properties
--DrawType
--TexWhite? or database value if not set? 
--ColorNameIndex
--DetailTexturePattern
--Transperancey
--FlagsHidden - dont draw!ignore it
--LightMode
--ShaderIndex?

Where to store? Database?

Swap coordinates so world up is y and not z. This could be done when parsing the file to keep things simple?