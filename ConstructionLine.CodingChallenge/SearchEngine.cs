using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private List<ColorCount> _colorCounts;
        private List<SizeCount> _sizeCounts;

        public Dictionary<string, int> _totalShirtsByColor;
        public Dictionary<string, int> _totalShirtsBySize;

        //public List<Shirt> FilteredShirts;
        //public Dictionary<string, int> FilteredShirtsByColor;
        //public Dictionary<string, int> FilteredShirtsBySize;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            _totalShirtsByColor = PopulateTotalShirtsByColor();
            _totalShirtsBySize = PopulateTotalShirtsBySize();

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

            _colorCounts = GetDefaultColorCounts();
            _sizeCounts = GetDefaultSizeCounts();
        }

        #region Public Methods

        #region Total Shirts

        public List<Shirt> GetTotalShirts()
        {
            return _shirts;
        }

        public Dictionary<string, int> GetTotalShirtsByColor()
        {
            return _totalShirtsByColor;
        }

        public Dictionary<string, int> GetTotalShirtsBySize()
        {
            return _totalShirtsBySize;
        }

        #endregion

        #region Filtered Shirts

        public SearchResults Search(SearchOptions options)
        {
            //PopulateColorCounts(options);
            //PopulateSizeCounts(options);

            var sizeIds = options.Sizes.Select(s => s.Id).ToList();
            var colorIds = options.Colors.Select(c => c.Id).ToList();

            var shirts = _shirts.Where(s => (!sizeIds.Any() || sizeIds.Contains(s.Size.Id)) && (!colorIds.Any() || colorIds.Contains(s.Color.Id))).ToList();

            foreach (var shirt in shirts)
            {
                AddToColorCounts(shirt);
                AddToSizeCounts(shirt);
            }

            return new SearchResults
            {
                Shirts = shirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }

        #endregion

        #endregion

        #region Private Methods

        private Dictionary<string, int> PopulateTotalShirtsByColor()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var color in Color.All)
            {
                dictionary.Add(color.Name, _shirts.Count(s => s.Color.Name.Equals(color.Name)));
            }

            return dictionary;
        }

        private Dictionary<string, int> PopulateTotalShirtsBySize()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var size in Size.All)
            {
                dictionary.Add(size.Name, _shirts.Count(s => s.Size.Name.Equals(size.Name)));
            }

            return dictionary;
        }

        private void AddToSizeCounts(Shirt shirt)
        {
            SizeCount sizeCount = _sizeCounts.FirstOrDefault(s => s.Size.Id == shirt.Size.Id);

            if (sizeCount != null)
            {
                sizeCount.Count += 1;
            }
        }

        private void AddToColorCounts(Shirt shirt)
        {
            ColorCount colorCount = _colorCounts.FirstOrDefault(s => s.Color.Id == shirt.Color.Id);

            if (colorCount != null)
            {
                colorCount.Count += 1;
            }
        }

        //private void PopulateColorCounts(SearchOptions options)
        //{
        //    var message = string.Empty;

        //    //foreach (var color in options.Colors)
        //    foreach (var color in Color.All)
        //    {
        //        List<Shirt> shirts = _shirts.Where(s => s.Color.Id == color.Id).ToList();

        //        message += $"{color.Name} : {shirts.Count} {Environment.NewLine}";

        //        if (shirts.Any())
        //        {
        //            ColorCount colorCount = _colorCounts.FirstOrDefault(s => s.Color.Id == color.Id);

        //            if (colorCount != null)
        //            {
        //                colorCount.Count += shirts.Count;
        //            }
        //        }
        //    }
        //}

        //private void PopulateSizeCounts(SearchOptions options)
        //{
        //    //foreach (var size in options.Sizes)
        //    foreach (var size in Size.All)
        //    {
        //        List<Shirt> shirts = _shirts.Where(s => s.Size.Id == size.Id).ToList();

        //        if (shirts.Any())
        //        {
        //            SizeCount sizeCount = _sizeCounts.FirstOrDefault(s => s.Size.Id == size.Id);

        //            if (sizeCount != null)
        //            {
        //                sizeCount.Count += shirts.Count;
        //            }
        //        }
        //    }
        //}

        private List<ColorCount> GetDefaultColorCounts()
        {
            var colorCounts = new List<ColorCount>();

            foreach (var color in Color.All)
            {
                colorCounts.Add(new ColorCount { Color = color, Count = 0 });
            }

            return colorCounts;
        }

        private List<SizeCount> GetDefaultSizeCounts()
        {
            var sizeCounts = new List<SizeCount>();

            foreach (var size in Size.All)
            {
                sizeCounts.Add(new SizeCount { Size = size, Count = 0 });
            }

            return sizeCounts;
        }

        #endregion
    }
}