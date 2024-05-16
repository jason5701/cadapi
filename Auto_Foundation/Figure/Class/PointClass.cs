using Autodesk.AutoCAD.Geometry;

public class PointClass
{
    private Point3d p1;
    private Point3d p2;
    public PointClass(Point3d p1, Point3d p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
    public Point3d P1
    { get { return p1; } set { p1 = value; } }
    public Point3d P2
    {
        get { return p2; }
        set { p2 = value; }
    }
}