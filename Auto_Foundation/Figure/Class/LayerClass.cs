using Color = Autodesk.AutoCAD.Colors.Color;
using Autodesk.AutoCAD.DatabaseServices;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Collections.Generic;

public class LayerClass: INotifyPropertyChanged
{
    private string layerName;
    private Color color;
    private short colorIndex;
    private string colorValue;
    private string type;
    private LineWeight weight;
    private bool plot;
    private string description;
    private LayerTableRecord ltr; 

    private List<string> linetypes;
    private List<LineWeight> lineweights;
    private List<bool> yesorno;

    public event PropertyChangedEventHandler PropertyChanged;
    public LayerClass
        (
            LayerTableRecord ltr,
            string layerName,
            string type
        )
    {
        this.ltr = ltr;
        this.layerName = layerName;
        this.type = type;

        this.color = ltr.Color;
        this.colorIndex = ltr.Color.ColorIndex;
        this.weight = ltr.LineWeight;
        this.plot = ltr.IsPlottable;
        this.description = ltr.Description;

        this.linetypes = ClCAD.GetAllLinetypes();
        this.lineweights = ClCAD.GetAllLineWeights();
        this.yesorno = new List<bool>();
        this.yesorno.Add(true); 
        this.yesorno.Add(false); 
        UpdateColorValue();
    }
    public List<LineWeight> Lineweights
    {
        get { return lineweights; }
        set { lineweights = value; }
    }
    public List<string> Linetypes
    {
        get { return linetypes; }
        set { linetypes = value; }
    }
    public List<bool> Yesorno 
    {
        get { return yesorno; }
        set { yesorno = value; }
    }
    public LayerTableRecord Ltr
    {
        get { return ltr; }
        set 
        { 
            ltr = value;
            OnPropertyChanged(nameof(Ltr));
        }
    }
    public Color Color
    {
        get { return color; }
        set
        {
            color = value;
            OnPropertyChanged(nameof(Color));
            UpdateColorValue();
        }
    }
    public string LayerName 
    { 
        get { return layerName; } 
        set 
        { 
            layerName = value;
            OnPropertyChanged(nameof(LayerName));
        } 
    }
    public short ColorIndex 
    { 
        get { return colorIndex; } 
        set 
        {
            colorIndex = value;
            OnPropertyChanged(nameof(ColorIndex));
            UpdateColorValue();

        } }
    public string Type
    {
        get { return type; }
        set
        {
            type = value;
            OnPropertyChanged(nameof(Type));
        }
    }
    public LineWeight Weight 
    { 
        get { return weight; }
        set 
        {
            weight = value;
            OnPropertyChanged(nameof(Weight));
        } 
    }
    public bool Plot
    {
        get { return plot; }
        set
        {
            plot = value;
            OnPropertyChanged(nameof(Plot));
        }
    }
    public string Description
    {
        get { return description; }
        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));
        }
    }


    public string ColorValue
    {
        get { return colorValue; }
        private set
        {
            colorValue = value;
            OnPropertyChanged(nameof(ColorValue));
        }
    }

    private void UpdateColorValue()
    {
        Color colorFromIndex = Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, ColorIndex);
        System.Windows.Media.Color wpfColor = System.Windows.Media.Color.FromRgb(
           Color.ColorValue.R,
            Color.ColorValue.G,
            Color.ColorValue.B);
        ColorValue = new SolidColorBrush(wpfColor).ToString();
    }
    private void OnPropertyChanged(string name) 
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}