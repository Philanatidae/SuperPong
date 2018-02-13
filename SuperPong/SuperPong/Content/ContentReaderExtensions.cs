/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
namespace SuperPong.Content
{
	public static class ContentReaderExtensions
	{
		private static readonly FieldInfo _contentReaderGraphicsDeviceFieldInfo = typeof(ContentReader).GetTypeInfo().GetDeclaredField("graphicsDevice");
		private static byte[] _scratchBuffer;

		public static GraphicsDevice GetGraphicsDevice(this ContentReader contentReader)
		{
			return (GraphicsDevice)_contentReaderGraphicsDeviceFieldInfo.GetValue(contentReader);
		}

		public static string GetRelativeAssetName(this ContentReader contentReader, string relativeName)
		{
			var assetDirectory = Path.GetDirectoryName(contentReader.AssetName);
			var assetName = Path.Combine(assetDirectory, relativeName).Replace('\\', '/');

			var ellipseIndex = assetName.IndexOf("/../", StringComparison.Ordinal);
			while (ellipseIndex != -1)
			{
				var lastDirectoryIndex = assetName.LastIndexOf('/', ellipseIndex - 1);
				if (lastDirectoryIndex == -1)
					lastDirectoryIndex = 0;
				assetName = assetName.Remove(lastDirectoryIndex, ellipseIndex + 4);
				ellipseIndex = assetName.IndexOf("/../", StringComparison.Ordinal);
			}

			return assetName;
		}

		internal static byte[] GetScratchBuffer(this ContentReader contentReader, int size)
		{
			size = Math.Max(size, 1024 * 1024);
			if (_scratchBuffer == null || _scratchBuffer.Length < size)
				_scratchBuffer = new byte[size];
			return _scratchBuffer;
		}
	}
}
