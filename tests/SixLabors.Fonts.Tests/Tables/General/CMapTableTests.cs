﻿using System.Linq;
using SixLabors.Fonts.Exceptions;
using SixLabors.Fonts.Tables.General;
using SixLabors.Fonts.Tables.General.CMap;
using SixLabors.Fonts.WellKnownIds;

using Xunit;

namespace SixLabors.Fonts.Tests.Tables.General
{
    public class CMapTableTests
    {
        [Fact]
        public void LoadFormat0()
        {
            var writer = new BinaryWriter();

            writer.WriteCMapTable(new[] {
                new Format0SubTable(0, PlatformIDs.Windows, 9, new byte[] { 0, 1, 2 })
            });

            CMapTable table = CMapTable.Load(writer.GetReader());

            Assert.Single(table.Tables.Where(x => x != null));

            Format0SubTable[] format0Tables = table.Tables.OfType<Format0SubTable>().ToArray();
            Assert.Single(format0Tables);
        }

        [Fact]
        public void ShouldThrowExceptionWhenTableCouldNotBeFound()
        {
            var writer = new BinaryWriter();
            writer.WriteTrueTypeFileHeader();

            using (var stream = writer.GetStream())
            {
                var exception = Assert.Throws<InvalidFontTableException>(() => CMapTable.Load(new FontReader(stream)));

                Assert.Equal("cmap", exception.Table);
            }
        }
    }
}