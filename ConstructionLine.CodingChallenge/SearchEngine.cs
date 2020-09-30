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

        private Dictionary<string, int> _totalShirtsCountByColor;
        private Dictionary<string, int> _totalShirtsCountBySize;

        private Dictionary<string, IEnumerable<Shirt>> _totalShirtsByColor;
        private Dictionary<string, IEnumerable<Shirt>> _totalShirtsBySize;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            
            PopulateTotalShirtsByColor();
            PopulateTotalShirtsBySize();
        }

        #region Public Methods

        #region Total Shirts

        public List<Shirt> GetTotalShirts()
        {
            return _shirts;
        }

        public Dictionary<string, int> GetTotalShirtsByColor()
        {
            return _totalShirtsCountByColor;
        }

        public Dictionary<string, int> GetTotalShirtsBySize()
        {
            return _totalShirtsCountBySize;
        }

        #endregion

        #region Filtered Shirts

        public SearchResults Search(SearchOptions options)
        {
            PopulateColorCounts(options);
            PopulateSizeCounts(options);

            var sizeIds = options.Sizes.Select(s => s.Id);
            var colorIds = options.Colors.Select(c => c.Id);

            var shirts = _shirts.Where(s => (!sizeIds.Any() || sizeIds.Contains(s.Size.Id)) && (!colorIds.Any() || colorIds.Contains(s.Color.Id)));

            return new SearchResults
            {
                Shirts = shirts.ToList(),
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }

        #endregion

        #endregion

        #region Private Methods

        private void PopulateTotalShirtsByColor()
        {
            _totalShirtsCountByColor = new Dictionary<string, int>();
            _totalShirtsByColor = new Dictionary<string, IEnumerable<Shirt>>();

            foreach (var color in Color.All)
            {
                var shirts = _shirts.Where(s => s.Color.Name.Equals(color.Name));

                _totalShirtsByColor.Add(color.Name, shirts);
                _totalShirtsCountByColor.Add(color.Name, shirts.Count());
            }
        }

        private void PopulateTotalShirtsBySize()
        {
            _totalShirtsCountBySize = new Dictionary<string, int>();
            _totalShirtsBySize = new Dictionary<string, IEnumerable<Shirt>>();

            foreach (var size in Size.All)
            {
                var shirts = _shirts.Where(s => s.Size.Name.Equals(size.Name));

                _totalShirtsBySize.Add(size.Name, shirts);
                _totalShirtsCountBySize.Add(size.Name, shirts.Count());
            }
        }

        private void PopulateColorCounts(SearchOptions options)
        {
            _colorCounts = new List<ColorCount>();

            foreach (var color in Color.All)
            {
                // Get shirts from a collection which were already aggregated by color
                var shirts = _totalShirtsByColor[color.Name];
                var colorCount = shirts.Count(c => (!options.Sizes.Any() || options.Sizes.Select(s => s.Id).Contains(c.Size.Id)));

                _colorCounts.Add(new ColorCount { Color = color, Count = colorCount });
            }
        }

        private void PopulateSizeCounts(SearchOptions options)
        {
            _sizeCounts = new List<SizeCount>();

            foreach (var size in Size.All)
            {
                // Get shirts from a collection which were already aggregated by size
                var shirts = _totalShirtsBySize[size.Name];
                var sizeCount = shirts.Count(s => (!options.Colors.Any() || options.Colors.Select(c => c.Id).Contains(s.Color.Id)));

                _sizeCounts.Add(new SizeCount { Size = size, Count = sizeCount });
            }
        }
        
        #endregion
    }
}