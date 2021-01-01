using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Xna.Framework;

// Originally from gist by Noel Berry
// https://gist.github.com/NoelFB/778d190e5d17f1b86ebf39325346fcc5/revisions
// Added few little modifications

// File Format:
// https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md

// Note: I didn't test with with Indexed or Grayscale colors
// Only implemented the stuff I needed / wanted, other stuff is ignored

namespace kuujoo.Pixel
{
    public partial class AseSprite
    {

        public enum Modes
        {
            Indexed = 1,
            Grayscale = 2,
            RGBA = 4
        }

        private enum Chunks
        {
            OldPaletteA = 0x0004,
            OldPaletteB = 0x0011,
            Layer = 0x2004,
            Cel = 0x2005,
            CelExtra = 0x2006,
            Mask = 0x2016,
            Path = 0x2017,
            FrameTags = 0x2018,
            Palette = 0x2019,
            UserData = 0x2020,
            Slice = 0x2022
        }

        public readonly Modes Mode;
        public readonly int Width;
        public readonly int Height;
        public readonly int FrameCount;

        public List<AseLayer> Layers = new List<AseLayer>();
        public List<AseFrame> Frames = new List<AseFrame>();
        public List<AseTag> Tags = new List<AseTag>();
        public List<AseSlice> Slices = new List<AseSlice>();

        public AseSprite(Modes mode, int width, int height)
        {
            Mode = mode;
            Width = width;
            Height = height;
        }

        public AseSprite(string file, bool addHiddenLayersToFrame = true)
        {
            using (var stream = Path.IsPathRooted(file) ? File.OpenRead(file) : TitleContainer.OpenStream(file))
            {
                var reader = new BinaryReader(stream);

                // wrote these to match the documentation names so it's easier (for me, anyway) to parse
                byte BYTE() { return reader.ReadByte(); }
                ushort WORD() { return reader.ReadUInt16(); }
                short SHORT() { return reader.ReadInt16(); }
                uint DWORD() { return reader.ReadUInt32(); }
                long LONG() { return reader.ReadInt32(); }
                string STRING() { return Encoding.UTF8.GetString(BYTES(WORD())); }
                byte[] BYTES(int number) { return reader.ReadBytes(number); }
                void SEEK(int number) { reader.BaseStream.Position += number; }

                // Header
                {
                    // file size
                    DWORD();

                    // Magic number (0xA5E0)
                    var magic = WORD();
                    if (magic != 0xA5E0)
                        throw new Exception("File is not in .ase format");

                    // Frames / Width / Height / Color Mode
                    FrameCount = WORD();
                    Width = WORD();
                    Height = WORD();
                    Mode = (Modes)(WORD() / 8);

                    // Other Info, Ignored
                    DWORD();       // Flags
                    WORD();        // Speed (deprecated)
                    DWORD();       // Set be 0
                    DWORD();       // Set be 0
                    BYTE();        // Palette entry 
                    SEEK(3);       // Ignore these bytes
                    WORD();        // Number of colors (0 means 256 for old sprites)
                    BYTE();        // Pixel width
                    BYTE();        // Pixel height
                    SEEK(92);      // For Future
                }

                // temporary variables
                var temp = new byte[Width * Height * (int)Mode];
                var palette = new Color[256];
                IAseUserData last = null;

                // Frames
                for (int i = 0; i < FrameCount; i++)
                {
                    var frame = new AseFrame(this);
                    frame.Pixels = new Color[Width * Height];
                    Frames.Add(frame);

                    long frameStart, frameEnd;
                    int chunkCount;

                    // frame header
                    {
                        frameStart = reader.BaseStream.Position;
                        frameEnd = frameStart + DWORD();
                        WORD();                  // Magic number (always 0xF1FA)
                        chunkCount = WORD();     // Number of "chunks" in this frame
                        frame.Duration = WORD(); // Frame duration (in milliseconds)
                        SEEK(6);                 // For future (set to zero)
                    }

                    // chunks
                    for (int j = 0; j < chunkCount; j++)
                    {
                        long chunkStart, chunkEnd;
                        Chunks chunkType;

                        // chunk header
                        {
                            chunkStart = reader.BaseStream.Position;
                            chunkEnd = chunkStart + DWORD();
                            chunkType = (Chunks)WORD();
                        }

                        // LAYER CHUNK
                        if (chunkType == Chunks.Layer)
                        {
                            // create layer
                            var layer = new AseLayer();

                            // get layer data
                            layer.Flag = (AseLayer.Flags)WORD();
                            layer.Type = (AseLayer.Types)WORD();
                            layer.ChildLevel = WORD();
                            WORD(); // width (unused)
                            WORD(); // height (unused)
                            layer.BlendMode = WORD();
                            layer.Alpha = (BYTE() / 255f);
                            SEEK(3); // for future
                            layer.Name = STRING();

                            last = layer;
                            Layers.Add(layer);
                        }
                        // CEL CHUNK
                        else if (chunkType == Chunks.Cel)
                        {
                            // create cel
                            var cel = new AseCel();

                            // get cel data
                            cel.Layer = Layers[WORD()];
                            cel.X = SHORT();
                            cel.Y = SHORT();
                            cel.Alpha = BYTE() / 255f;
                            var celType = WORD(); // type
                            SEEK(7);

                            // RAW or DEFLATE
                            if (celType == 0 || celType == 2)
                            {
                                cel.Width = WORD();
                                cel.Height = WORD();

                                var count = cel.Width * cel.Height * (int)Mode;

                                // RAW
                                if (celType == 0)
                                {
                                    reader.Read(temp, 0, cel.Width * cel.Height * (int)Mode);
                                }
                                // DEFLATE
                                else
                                {
                                    SEEK(2);

                                    var deflate = new DeflateStream(reader.BaseStream, CompressionMode.Decompress);
                                    deflate.Read(temp, 0, count);
                                }

                                cel.Pixels = new Color[cel.Width * cel.Height];
                                BytesToPixels(temp, cel.Pixels, Mode, palette);
                                if (addHiddenLayersToFrame || cel.Layer.Flag.HasFlag(AseLayer.Flags.Visible)) {
                                    CelToFrame(frame, cel);
                                }
                            }
                            // REFERENCE
                            else if (celType == 1)
                            {
                                // not gonna worry about it
                            }

                            last = cel;
                            frame.Cels.Add(cel);
                        }
                        // PALETTE CHUNK
                        else if (chunkType == Chunks.Palette)
                        {
                            var size = DWORD();
                            var start = DWORD();
                            var end = DWORD();
                            SEEK(8); // for future

                            for (int p = 0; p < (end - start) + 1; p++)
                            {
                                var hasName = WORD();
                                palette[start + p] = Color.FromNonPremultiplied(BYTE(), BYTE(), BYTE(), BYTE());
                                if (IsBitSet(hasName, 0))
                                    STRING();
                            }
                        }
                        // USERDATA
                        else if (chunkType == Chunks.UserData)
                        {
                            if (last != null)
                            {
                                var flags = (int)DWORD();

                                // has text
                                if (IsBitSet(flags, 0))
                                    last.UserDataText = STRING();

                                // has color
                                if (IsBitSet(flags, 1))
                                    last.UserDataColor = Color.FromNonPremultiplied(BYTE(), BYTE(), BYTE(), BYTE());
                            }
                        }
                        // TAG
                        else if (chunkType == Chunks.FrameTags)
                        {
                            var count = WORD();
                            SEEK(8);

                            for (int t = 0; t < count; t++)
                            {
                                var tag = new AseTag();
                                tag.From = WORD();
                                tag.To = WORD();
                                tag.LoopDirection = (AseTag.LoopDirections)BYTE();
                                SEEK(8);
                                tag.Color = Color.FromNonPremultiplied(BYTE(), BYTE(), BYTE(), 255);
                                SEEK(1);
                                tag.Name = STRING();
                                Tags.Add(tag);
                            }
                        }
                        // SLICE
                        else if (chunkType == Chunks.Slice)
                        {
                            var count = DWORD();
                            var flags = (int)DWORD();
                            DWORD(); // reserved
                            var name = STRING();

                            for (int s = 0; s < count; s++)
                            {
                                var slice = new AseSlice();
                                slice.Name = name;
                                slice.Frame = (int)DWORD();
                                slice.OriginX = (int)LONG();
                                slice.OriginY = (int)LONG();
                                slice.Width = (int)DWORD();
                                slice.Height = (int)DWORD();

                                // 9 slice (ignored atm)
                                if (IsBitSet(flags, 0))
                                {
                                    LONG();
                                    LONG();
                                    DWORD();
                                    DWORD();
                                }

                                // pivot point
                                if (IsBitSet(flags, 1))
                                    slice.Pivot = new Vector2((int)DWORD(), (int)DWORD());

                                last = slice;
                                Slices.Add(slice);
                            }
                        }

                        reader.BaseStream.Position = chunkEnd;
                    }

                    reader.BaseStream.Position = frameEnd;
                }
            }
        }

        /// <summary>
        /// Converts an array of Bytes to an array of Colors, using the specific Aseprite Mode & Palette
        /// </summary>
        private void BytesToPixels(byte[] bytes, Color[] pixels, AseSprite.Modes mode, Color[] palette)
        {
            int len = pixels.Length;
            if (mode == Modes.RGBA)
            {
                for (int p = 0, b = 0; p < len; p++, b += 4)
                {
                    pixels[p].R = (byte)(bytes[b + 0] * bytes[b + 3] / 255);
                    pixels[p].G = (byte)(bytes[b + 1] * bytes[b + 3] / 255);
                    pixels[p].B = (byte)(bytes[b + 2] * bytes[b + 3] / 255);
                    pixels[p].A = bytes[b + 3];
                }
            }
            else if (mode == Modes.Grayscale)
            {
                for (int p = 0, b = 0; p < len; p++, b += 2)
                {
                    pixels[p].R = pixels[p].G = pixels[p].B = (byte)(bytes[b + 0] * bytes[b + 1] / 255);
                    pixels[p].A = bytes[b + 1];
                }
            }
            else if (mode == Modes.Indexed)
            {
                for (int p = 0, b = 0; p < len; p++, b += 1)
                    pixels[p] = palette[b];
            }
        }

        /// <summary>
        /// Applies a Cel's pixels to the Frame, using its Layer's BlendMode & Alpha
        /// </summary>
        private void CelToFrame(AseFrame frame, AseCel cel)
        {
            var opacity = (byte)((cel.Alpha * cel.Layer.Alpha) * 255);
            for (int sx = 0; sx < cel.Width; sx++)
            {
                int dx = cel.X + sx;
                int dy = cel.Y * frame.Sprite.Width;

                for (int i = 0, sy = 0; i < cel.Height; i++, sy += cel.Width, dy += frame.Sprite.Width)
                {
                    AseBlends.Blend(cel.Layer.BlendMode, ref frame.Pixels[dx + dy], cel.Pixels[sx + sy], opacity);
                }
            }
        }

        private static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
    }
}