# dCHR Format

Reverse engineer based of `Dragon Ball Z: Attack of the Saiyans` game.
File for rendering sprites into the screen.

**File content:**
```
FileHeader
Palettes[NumPalettes]
TilesData[NumTiles]
(SpriteData)
```

## File Header

Header is 8 bytes.

```
uint32: File ID, string "dCHR"
 uint8: File flags
        00 = Palette and tiles
        02 = Palette, tiles and data
 uint8: Number of palettes
uint16: Number of tiles
```

## Palette

Palette is 32 bytes each.
16 colors.
Uncompressed RGB555 format.

## Tiles Data

Each tile is 32 bytes each.
Uncompressed 4bpp format.

## Sprite Data

File may or may not contain this data.
Specify how the sprites should be rendered.

```
uint32: Number of sprites
<Each Sprite>
  uint16: Offset after number of cells

<At offset>
  uint8: Number of layers
  uint8: Unknown
  <Each layer>
    sint8: Signed X offset
    sint8: Signed Y offset
    uint8: Tile (lower 8-bits)
    uint8: Control
    - bit 6~7: OBJ size
    - bit 5  : Vertical flip
    - bit 4  : Horizontal flip
    - bit 2~3: OBJ shape
    - bit 1  : *Semi-transparent
    - bit 0  : *Tile (higher 1-bit)
```

\*: If both bits 0 and 1 are set, data will behave differently. Sadly how that works is still unknown.

Layers are rendered from last to first. e.g. first on top.

**OBJ size and shape:**
|           | Size 0 | Size 1 | Size 2 | Size 3 |
|-----------|--------|--------|--------|--------|
|**Shape 0**| 8x8    | 16x16  | 32x32  | 64x64  |
|**Shape 1**| 16x8   | 32x8   | 32x16  | 64x32  |
|**Shape 2**| 8x16   | 8x32   | 16x32  | 32x64  |
|**Shape 3**| N/A    | N/A    | N/A    | N/A    |

## Credits

Revision 1 @ 2022/02/12 by Justburner.
