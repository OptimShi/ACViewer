using ACE.DatLoader;
using ACViewer.Entity;
using SharpDX;
using System.Collections.Generic;

namespace ACViewer.FileTypes
{
    public class SurfaceTexture
    {
        public ACE.DatLoader.FileTypes.SurfaceTexture _surfaceTexture;

        public SurfaceTexture(ACE.DatLoader.FileTypes.SurfaceTexture surfaceTexture)
        {
            _surfaceTexture = surfaceTexture;
        }

        public TreeNode BuildTree()
        {
            var treeView = new TreeNode($"{_surfaceTexture.Id:X8}");

            if (DatManager.DatVersion == DatVersionType.DM)
            {
                var format = new TreeNode($"Format: {_surfaceTexture.Format}");
                var width = new TreeNode($"Width: {_surfaceTexture.Width}");
                var height = new TreeNode($"Height: {_surfaceTexture.Height}");

                treeView.Items.AddRange(new List<TreeNode>() { format, width, height });

                if (_surfaceTexture.DefaultPaletteId != null)
                    treeView.Items.Add(new TreeNode($"DefaultPalette: {_surfaceTexture.DefaultPaletteId:X8}", clickable: true));
            }
            else
            {
                var unknown = new TreeNode($"Unknown: {_surfaceTexture.Unknown}");
                var unknownByte = new TreeNode($"UnknownByte: {_surfaceTexture.UnknownByte}");

                var textures = new TreeNode("Textures:");
                foreach (var textureID in _surfaceTexture.Textures)
                    textures.Items.Add(new TreeNode($"{textureID:X8}", clickable: true));

                treeView.Items.AddRange(new List<TreeNode>() { unknown, unknownByte, textures });
            }
            return treeView;
        }
    }
}
