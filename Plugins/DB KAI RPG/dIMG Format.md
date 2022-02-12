# dIMG Format

Reverse engineer based of `Dragon Ball Z: Attack of the Saiyans` game.
File for rendering textures into the screen.

**File content:**
```
FileHeader
Palettes[NumPalettes]
Texture
```

## File Header

Header is 8 bytes.

```
uint32: File ID, string "dIMG"
 uint8: Unknown, always zero
 uint8: Palette flags
        00 = 16 Palettes
        03 = 1 Palette
 uint8: Texture Width divided by 4
 uint8: Texture Height divided by 2
```

## Palette

Palette is 32 bytes each.
16 colors.
Uncompressed RGB555 format.

## Texture

Texture is linear uncompressed 4bpp format.
Lower nibble on left side.

## Credits

Revision 1 @ 2022/02/12 by Justburner.
