using System.Collections;
using System.Collections.Generic;

public class DrawClass
{
    private string dwgName;
    private string plotStyle;
    private string filePath;

    public DrawClass(string dwgName, string plotStyle, string filePath)
    {
        this.dwgName = dwgName;
        this.plotStyle = plotStyle;
        this.filePath = filePath;
    }
    public string DwgName
    {
        get { return dwgName; }
        set { dwgName = value; }
    }
    public string PlotStyle
    { get { return plotStyle; } set { plotStyle = value; } }
    public string FilePath {  get { return filePath; } set {  filePath = value; } }
}

public class DrawClassComparer: IEqualityComparer<DrawClass>
{
    public bool Equals(DrawClass x, DrawClass y)
    {
        if(object.ReferenceEquals(x, y)) return true;
        if(object.ReferenceEquals(x, null)||object.ReferenceEquals(y,null)) return false;
        return x.FilePath == y.FilePath;
    }

    public int GetHashCode(DrawClass obj)
    {
        if (obj == null) return 0;
        int FilePathHasCode = obj.FilePath.GetHashCode();
        return FilePathHasCode;
    }
}