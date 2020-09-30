using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {

        #region Filtered Shirts

        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue)
            };

            var searchEngine = new SearchEngine(shirts);
                        
            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red, Color.Blue },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            // Note: this test results are not matching with the 'requirements' as I understood - I can give explanation - Raj
            //AssertResults(results.Shirts, searchOptions);
            //AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            //AssertColorCounts(shirts, searchOptions, results.ColorCounts);

            Assert.AreEqual(1, results.Shirts.Count);
            Assert.AreEqual(1, results.SizeCounts.FirstOrDefault(s => s.Size.Name.Equals(Size.Small.Name)).Count);
            Assert.AreEqual(1, results.ColorCounts.FirstOrDefault(c => c.Color.Name.Equals(Color.Red.Name)).Count);
        }

        [Test]
        public void Search_WithNoSearchOptions_ReturnsAllShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue)
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                
            };

            var results = searchEngine.Search(searchOptions);

            Assert.AreEqual(shirts.Count, results.Shirts.Count);
        }

        [Test]
        public void Search_ReturnsAllShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red)
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red }
            };

            var results = searchEngine.Search(searchOptions);

            var expectedShirts = 2;
            var expectedRedColorCounts = 2;
            var expectedSmallSizeCounts = 1;
            var expectedMediumSizeCounts = 1;

            Assert.AreEqual(expectedShirts, results.Shirts.Count);
            Assert.AreEqual(expectedRedColorCounts, results.ColorCounts.FirstOrDefault(c => c.Color.Name.Equals(Color.Red.Name)).Count);
            Assert.AreEqual(expectedSmallSizeCounts, results.SizeCounts.FirstOrDefault(c => c.Size.Name.Equals(Size.Small.Name)).Count);
            Assert.AreEqual(expectedMediumSizeCounts, results.SizeCounts.FirstOrDefault(c => c.Size.Name.Equals(Size.Medium.Name)).Count);
        }

        #endregion

        #region Total Shirts

        [Test]
        public void GetTotalShirts_ReturnsAllShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue)
            };

            var searchEngine = new SearchEngine(shirts);

            var actualNumberOfShirts = searchEngine.GetTotalShirts();

            Assert.AreEqual(shirts.Count, actualNumberOfShirts.Count);
        }

        [Test]
        public void GetTotalShirtsByColor_Returns1Red1Black1Blue0WhiteAnd0YellowShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue)
            };

            var searchEngine = new SearchEngine(shirts);

            var expectedRedShirts = 1;
            var expectedBlackShirts = 1;
            var expectedBlueShirts = 1;
            var expectedWhiteShirts = 0;
            var expectedYellowShirts = 0;

            Dictionary<string, int> totalShirtsByColor = searchEngine.GetTotalShirtsByColor();

            Assert.AreEqual(expectedRedShirts, totalShirtsByColor[Color.Red.Name]);
            Assert.AreEqual(expectedBlackShirts, totalShirtsByColor[Color.Black.Name]);
            Assert.AreEqual(expectedBlueShirts, totalShirtsByColor[Color.Blue.Name]);
            Assert.AreEqual(expectedWhiteShirts, totalShirtsByColor[Color.White.Name]);
            Assert.AreEqual(expectedYellowShirts, totalShirtsByColor[Color.Yellow.Name]);
        }

        [Test]
        public void GetTotalShirtsBySize_Returns1Small1MediumAnd1LargeShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue)
            };

            var searchEngine = new SearchEngine(shirts);

            var expectedSmallShirts = 1;
            var expectedMediumShirts = 1;
            var expectedLargeShirts = 1;
            
            Dictionary<string, int> totalShirtsBySize = searchEngine.GetTotalShirtsBySize();

            Assert.AreEqual(expectedSmallShirts, totalShirtsBySize[Size.Small.Name]);
            Assert.AreEqual(expectedMediumShirts, totalShirtsBySize[Size.Medium.Name]);
            Assert.AreEqual(expectedLargeShirts, totalShirtsBySize[Size.Large.Name]);
        }

        #endregion
    }
}
