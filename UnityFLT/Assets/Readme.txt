

Supported Texture formats:

png
jpg
rgb - to be done
rgba - to be done
int - to be done


TODO:

-Records:-

LOD
External Ref


-Some notes to myself:-

LOD node center, is this relative to position or is it the actual position? Check it and adjust the LOD comments and update function.

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
--LightMode
--ShaderIndex?
--Support for extended materials

Taken into account:
--Face hidden flag

Where to store? Database?

Swap coordinates so world up is y and not z. This could be done when parsing the file to keep things simple?