using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using System.Collections.Generic;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

public abstract class EntitiesJig : DrawJig
{
    protected IEnumerable<Entity> entities;
    protected Point3d basePoint;

    public abstract Matrix3d Transform { get; }

    public EntitiesJig(IEnumerable<Entity> entities, Point3d basePoint)
    {
        this.entities = entities;
        this.basePoint = basePoint;
    }

    protected override bool WorldDraw(WorldDraw draw)
    {
        WorldGeometry geometry = draw.Geometry;
        if (geometry != null)
        {
            geometry.PushModelTransform(Transform);
            foreach (Entity ent in entities)
            {
                geometry.Draw(ent);
            }
            geometry.PopModelTransform();
        }
        return true;
    }
}

public class MoveJig : EntitiesJig
{
    readonly bool rubberBand;
    string message;

    public Point3d Point { get; private set; }

    public override Matrix3d Transform =>
        Matrix3d.Displacement(basePoint.GetVectorTo(Point));

    public MoveJig(IEnumerable<Entity> entities, Point3d basePoint, string message, bool rubberBand = false)
        : base(entities, basePoint)
    {
        this.message = message;
        this.rubberBand = rubberBand;
    }

    protected override SamplerStatus Sampler(JigPrompts prompts)
    {
        var options = new JigPromptPointOptions(message)
        {
            BasePoint = basePoint,
            UseBasePoint = true
        };
        if (rubberBand)
            options.Cursor = CursorType.RubberBand;
        options.UserInputControls =
            UserInputControls.Accept3dCoordinates;
        PromptPointResult result = prompts.AcquirePoint(options);
        if (result.Value.IsEqualTo(Point))
            return SamplerStatus.NoChange;
        Point = result.Value;
        return SamplerStatus.OK;
    }
}

public class RotateJig : EntitiesJig
{
    Vector3d axis;
    string message;

    public double Angle { get; private set; }

    public override Matrix3d Transform =>
        Matrix3d.Rotation(Angle, axis, basePoint);

    public RotateJig(IEnumerable<Entity> entities, Point3d basePoint, Vector3d axis, string message)
        : base(entities, basePoint)
    {
        this.axis = axis;
        this.message = message;
    }

    protected override SamplerStatus Sampler(JigPrompts prompts)
    {
        var options = new JigPromptAngleOptions(message)
        {
            BasePoint = basePoint,
            UseBasePoint = true,
            Cursor = CursorType.RubberBand,
            DefaultValue = 0.0,
            UserInputControls =
            UserInputControls.NullResponseAccepted |
            UserInputControls.Accept3dCoordinates |
            UserInputControls.GovernedByUCSDetect
        };
        PromptDoubleResult result = prompts.AcquireAngle(options);
        if (result.Value == Angle)
            return SamplerStatus.NoChange;
        Angle = result.Value;
        return SamplerStatus.OK;
    }

    public class RectangleJig : DrawJig
    {
        private Point3d startPoint;
        private Point3d currentPoint;

        public RectangleJig(Point3d firstPoint)
        {
            startPoint = firstPoint;
            currentPoint = firstPoint;
        }

        public List<Point3d> GetCornerPoints()
        {
            var points = new List<Point3d>();
            points.Add(startPoint);
            points.Add(new Point3d(startPoint.X, currentPoint.Y, 0));
            points.Add(currentPoint);
            points.Add(new Point3d(currentPoint.X, startPoint.Y, 0));
            return points;
        }
        public Point3d CurrentPoint => currentPoint;
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions("\nSelect opposite corner:");
            options.BasePoint = startPoint;
            options.UseBasePoint = true;
            options.UserInputControls =
            UserInputControls.Accept3dCoordinates;

            PromptPointResult result = prompts.AcquirePoint(options);
            if (result.Status == PromptStatus.OK && result.Value != currentPoint)
            {
                currentPoint = result.Value;
                return SamplerStatus.OK;
            }
            return SamplerStatus.NoChange;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            WorldGeometry geometry = draw.Geometry;
            if (geometry != null)
            {
                //Polyline rectangle = new Polyline(4);
                //rectangle.SetDatabaseDefaults();
                //rectangle.Color = Color.FromColorIndex(ColorMethod.ByAci, 7);
                //rectangle.Linetype = "Dashed";

                //rectangle.AddVertexAt(0, new Point2d(startPoint.X, startPoint.Y), 0, 0, 0);
                //rectangle.AddVertexAt(1, new Point2d(currentPoint.X, startPoint.Y), 0, 0, 0);
                //rectangle.AddVertexAt(2, new Point2d(currentPoint.X, currentPoint.Y), 0, 0, 0);
                //rectangle.AddVertexAt(3, new Point2d(startPoint.X, currentPoint.Y), 0, 0, 0);
                //rectangle.Closed = true;

                //geometry.Draw(rectangle);
                geometry.Polygon(new Point3dCollection(GetCornerPoints().ToArray()));
                geometry.PopModelTransform();
            }
            return true;
        }
    }
}