using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace Application.Models
{
    public class PilotInfo : ObservableValue
    {
        public PilotInfo(string name, double value, SolidColorPaint paint)
        {
            Name = name;
            Paint = paint;
            // the ObservableValue.Value property is used by the chart
            Value = value;
        }

        public string Name { get; set; }
        public SolidColorPaint Paint { get; set; }
    }
}
