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
        private SolidColorPaint solidColorPaint;

        public PilotInfo(string name, double value, SolidColorPaint paint)
        {
            Name = name;
            Paint = paint;
            // the ObservableValue.Value property is used by the chart
            Value = value;
        }

        public PilotInfo(string name, double? value, SolidColorPaint solidColorPaint)
        {
            Name = name;
            Value = value;
            this.solidColorPaint = solidColorPaint;
        }

        public string Name { get; set; }
        public SolidColorPaint Paint { get; set; }
    }
}
