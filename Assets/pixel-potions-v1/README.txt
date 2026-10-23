PIXEL POTIONS v1 - free animated pixel-art potions
==================================================

19 looping animations: 18 potions and a brewing cauldron, drawn at native size
(potions 64x64 px, cauldron 96x96 px), every frame 70 ms (about 14.3 frames a second),
transparent backgrounds.

FOLDERS
  sheets/          the game-ready sprite sheets (PNG, RGBA) and their atlases (JSON)
  sheets-no-halo/  the same loops without the outer glow, for engines that add their own
                   (series 2 and 3 potions except frost, and the cauldron)
  icons/           one still 64x64 icon per loop, for inventories and menus
  previews/        GIF previews on a dark background (6x; the cauldron 4x) and three shelf GIFs

ANIMATIONS
  potion-*   8 potions, 32 frames each (2.24 s loop): elixir, crimson, azure, amethyst,
             amber, frost, shadow, rainbow. Bubbles, turning rays, a cork that rattles,
             pops and lands.
  potion2-*  6 potions, 48 frames each (3.36 s loop): heartbeat, starlit, ember, frost,
             void, poison. Each bottle has its own behaviour.
  potion3-*  4 potions, 48 frames each (3.36 s loop): storm, crystal, wind, sun.
  brew-cauldron  64 frames (4.48 s): ingredients land, the brew swirls and flashes, and
                 a potion rises and pops its cork.

USING THE ATLASES
  Each atlas is a JSON hash in the common TexturePacker style: "frames" holds each
  frame's rectangle in the sheet and its "duration" in milliseconds, "animations.loop"
  lists the frames in playback order, and meta.image names the sheet. Phaser's
  load.atlas and PixiJS's Spritesheet read it. Frame names are unique across the pack
  (potion-elixir-00, potion-elixir-01, ...), so several atlases can share one texture
  cache. Without an atlas loader, slice the sheet into equal cells: 64x64 (the cauldron
  96x96), 8 per row, left to right, top to bottom.
  Scale only by whole numbers (2x, 3x, 4x) with nearest-neighbour filtering.

LICENCE
  CC0 1.0 Universal (public domain dedication): see LICENSE.txt. Use the art in any
  project, commercial or not, change it, no permission needed.
  Credit isn't required, but it is appreciated: "Pixel Potions by Skulitom Rivskiy,
  made with PixelForge (github.com/skulitom/PixelForge)".

HOW IT WAS MADE (AI DISCLOSURE)
  AI agents made this art. A language model (Anthropic's Claude) designed the potions and
  wrote the drawing code: shapes, colour ramps, dithered glow and particle effects as maths
  and small pixel grids written out in code, rendered with PixelForge, our free
  MIT-licensed pixel-art toolkit. No image model was used, and nothing was traced or
  sampled from anyone else's art. The publisher asked for the first potion and its colour
  variants, gave feedback, and approved the art before release.

LINKS
  PixelForge (make and edit sprites like these from JSON recipes):
    https://github.com/skulitom/PixelForge
  PixelForge Studio (free desktop app): https://skulitom.itch.io/pixelforge-studio
